using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StickyNoteUIItem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    
    public event System.Action<StickyNoteUIItem> OnItemPointerDown;
    public StickyNote sticky;

    public RectTransform rectTransform { get { return GetComponent<RectTransform>(); } }

    bool pointerDownInvoked;

    public void ApplyConfig(StickyNote sticky)
    {
        this.sticky = sticky;
        GetComponent<Image>().color = sticky.color;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!pointerDownInvoked)
        {
            OnItemPointerDown(this);
            pointerDownInvoked = true;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        pointerDownInvoked = false;
    }
}
