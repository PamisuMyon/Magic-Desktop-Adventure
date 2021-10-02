using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideSetting : MonoBehaviour
{
    public bool hideInTouchMode;
    public bool hideInNormalMode;

    void Start()
    {
        if (GameManager.Instance.isTouchInput)
            if (hideInTouchMode)
                gameObject.SetActive(false);
            else
                gameObject.SetActive(true);
        if (hideInNormalMode)
            gameObject.SetActive(false);
        else 
            gameObject.SetActive(true);
    }

}
