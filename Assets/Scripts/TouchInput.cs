using UnityEngine;

public class TouchInput : MonoBehaviour 
{

    public static TouchInput Instance { get; private set; }

    internal Joystick joystick;
    internal TouchButton jump;
    internal TouchButton interact;

    private void Start() 
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }    

        Instance = this;

        joystick = transform.Find("Joystick").GetComponent<Joystick>();
        jump = transform.Find("Jump").GetComponent<TouchButton>();
        interact = transform.Find("Interact").GetComponent<TouchButton>();

        if (!GameManager.Instance.isTouchInput)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
            joystick.gameObject.SetActive(true);
            jump.gameObject.SetActive(true);
            interact.gameObject.SetActive(true);
        }
    }
    
}