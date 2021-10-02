using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStickyNote : MonoBehaviour
{

    PlayerInput input;
    Vector3 inventoryPosition;

    Stickable currentStickable;
    StickyNotesContainer container { get { return StickyNotesContainer.Instance; } }

    void Start()
    {
        input = GetComponent<PlayerInput>();
        inventoryPosition = new Vector3(0, transform.position.y - 10, 0);

        TriggerArea triggerArea = GetComponentInChildren<TriggerArea>();
        triggerArea.TriggerEnter += OnInteractTriggerEnter;
        triggerArea.TriggerStay += OnInteractTriggerStay;
        triggerArea.TriggerExit += OnInteractTriggerExit;
    }

    void Update()
    {
        if (input.previous)
        {
            container.SelectPrevious();
            input.previous = false;
        }
        if (input.next)
        {
            container.SelectNext();
            input.next = false;
        }
        if (input.interact)
        {
            if (currentStickable)
            {
                if (currentStickable.CanStick)
                {
                    if (container.Current)
                    {
                        var sticky = container.RemoveCurrent();
                        currentStickable.StickOn(sticky);
                    }
                }
                else
                    currentStickable.Remove();
            }
            input.interact = false;
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.tag == "StickyNoteItem")
        {
            // The callback delayed causes the bug that restart level when picking up a sticky note item,
            // the item will be added to player's inventory after level restarts.
            // other.gameObject.GetComponent<StickyNotePickable>().AnimateToInventory(inventoryPosition, AddToInventory);

            // Use coroutine instead
            var pickable = other.gameObject.GetComponent<StickyNotePickable>();
            var sticky = pickable.AnimateToInventory(inventoryPosition);
            StartCoroutine(AddToInventoryDelay(sticky, pickable.animDuration0 + pickable.animDuration1));
        } else if (other.gameObject.tag == "Item")
        {
            other.gameObject.GetComponent<Gem>()?.Picked();
        }
    }

    private void OnInteractTriggerEnter(Collider other) 
    {
        if (other.gameObject.tag == "Obstacle")
        {
            Stickable stickable = other.GetComponent<Stickable>();
            if (currentStickable != stickable)
            {
                // If a stickable already exists, use the new one and hide the old one
                if (currentStickable)
                    currentStickable.HideHint();
                currentStickable = stickable;
                // Show hint when having sticky note in inventory or the stickable object has been sticked
                if (container.Current || !stickable.CanStick)
                    currentStickable.ShowHint();
            }
        }
    }

    private void OnInteractTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Obstacle")
        {
            Stickable stickable = other.GetComponent<Stickable>();
            if (currentStickable == stickable)
            {
                // Hide hint when run out of sticky notes and the stickable object has no stickies to remove
                if (!container.Current && stickable.CanStick)
                {
                    currentStickable.HideHint();
                }
            }
        }
    }

    private void OnInteractTriggerExit(Collider other) 
    {
        if (other.gameObject.tag == "Obstacle")
        {
            Stickable stickable = other.GetComponent<Stickable>();
            if (currentStickable == stickable)
            {
                stickable.HideHint();
                currentStickable = null;
            }
        }
    }

    void AddToInventory(StickyNote sticky)
    {
        StickyNotesContainer.Instance.AddItem(sticky);
    }

    IEnumerator AddToInventoryDelay(StickyNote sticky, float delay)
    {
        yield return new WaitForSeconds(delay);
        AddToInventory(sticky);
    }    
}
