using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

public class TouchButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public TouchButtonEvent OnButtonDown;
    public TouchButtonEvent OnButton;
    public TouchButtonEvent OnButtonUp;

    public bool isButtonDown;
    public bool isButton;
    public bool isButtonUp;

    bool downInvoked;

    [System.Serializable]
    public class TouchButtonEvent : UnityEvent {}

    private void LateUpdate() 
    {
        isButtonDown = false;
        isButtonUp = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isButtonDown = true;
        if (!downInvoked)
        {
            downInvoked = true;
            OnButtonDown.Invoke();
        }
        isButton = true;
        OnButton.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isButtonDown = false;
        isButton = false;
        downInvoked = false;
        if (!isButtonUp)
        {
            isButtonUp = true;
            OnButtonUp.Invoke();
        }
    }

}