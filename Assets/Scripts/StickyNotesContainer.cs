using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StickyNotesContainer : MonoBehaviour
{

    public static StickyNotesContainer Instance { get; private set; }

    public GameObject itemPrefab;
    public float selectedOffset;
    public float itemWidth;
    public float itemHeight;
    public float animateDuration;

    [Space]
    public AudioClip selectClip;

    public StickyNote Current { get 
    { 
        if (selected >= 0 && selected < items.Count)
            return items[selected].sticky;
        else
            return null;
    } }

    AudioSource audioSource;
    List<StickyNoteUIItem> items = new List<StickyNoteUIItem>();
    List<Vector3> targets = new List<Vector3>();    //  Tween target positions of sticky note ui items
    [SerializeField] int selected = -1;

    void Start()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;

        audioSource = GetComponent<AudioSource>();

        if (GameManager.Instance.isTouchInput)
        {
            GetComponent<RectTransform>().localScale = new Vector3(1.4f, 1.4f, 1.4f);
        }
    }

    public void AddItem(StickyNote sticky)
    {
        var newItem = Instantiate(itemPrefab, transform).GetComponent<StickyNoteUIItem>();
        newItem.ApplyConfig(sticky);
        newItem.OnItemPointerDown += OnItemPointerDown;
        items.Add(newItem);

        SelectItem(items.Count - 1);
        // New item's initial position should be at the bottom of screen
        var pos = targets[items.Count - 1];
        newItem.rectTransform.localPosition = new Vector3(pos.x, pos.y - selectedOffset - itemHeight, pos.z);
        
    }

    public void SelectItem(int index)
    {
        selected = index;
        AdjustStickies();
        if (items.Count != 0)
        {
            audioSource.clip = selectClip;
            audioSource.pitch = Random.Range(0.8f, 1.2f);
            audioSource.Play();
        }
    }

    public void SelectNext() => SelectItem(Mathf.Clamp(selected + 1, 0, items.Count - 1));
    public void SelectPrevious() => SelectItem(Mathf.Clamp(selected - 1, 0, items.Count - 1));

    void OnItemPointerDown(StickyNoteUIItem item)
    {
        var index = items.IndexOf(item);
        if (index >= 0)
        {
            SelectItem(index);
        }
    }

    public StickyNote RemoveCurrent()
    {
        var removedItem = items[selected];
        items.RemoveAt(selected);
        selected = Mathf.Clamp(selected, 0, items.Count - 1);
        
        // Animate removed item
        var pos = removedItem.rectTransform.localPosition;
        pos.Set(pos.x, 0 - itemHeight, pos.z);
        removedItem.rectTransform.DOLocalMove(pos, animateDuration)
            .OnComplete(() => Destroy(removedItem.gameObject));

        // Reposition stickies remain
        AdjustStickies();
        
        return removedItem.sticky;
    }

    private void AdjustStickies()
    {
        // Calculate stickies' final positions
        targets.Clear();
        var x = 0 - itemWidth * items.Count / 2 + itemWidth / 2;
        for (int i = 0; i < items.Count; i++)
        {
            var pos = new Vector3(x, 0, 0);
            if (i == selected)
                pos += new Vector3(0, selectedOffset, 0);
            targets.Add(pos);
            x += itemWidth;
        }
        
        if (targets.Count == 0) return;
        // Apply tweens
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].rectTransform.localPosition == targets[i])
                continue;
            items[i].rectTransform.DOLocalMove(targets[i], animateDuration);
        }
    }

}
