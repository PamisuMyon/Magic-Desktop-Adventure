using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{

    public bool jump;
    public bool jumpHeld;
    public float horizontal;
    public bool previous;
    public bool next;
    public bool interact;

    void Update()
    {
        if (GameManager.Instance.isTouchInput)
        {
            horizontal = TouchInput.Instance.joystick.Horizontal;

            if (TouchInput.Instance.jump.isButtonDown)
                jump = true;
            jumpHeld = TouchInput.Instance.jump.isButton;

            if (TouchInput.Instance.interact.isButtonDown)
                interact = true;
        }
        else
        {
            horizontal = Input.GetAxis("Horizontal");

            if (Input.GetButtonDown("Jump"))
                jump = true;
            jumpHeld = Input.GetButton("Jump");

            if (Input.GetButtonDown("Previous"))
                previous = true;
            if (Input.GetButtonDown("Next"))
                next = true;
            if (Input.GetButtonDown("Interact"))
                interact = true;
        }
    }

}
