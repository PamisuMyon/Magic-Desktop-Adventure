using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{

    public Color current;

    GameObject menuPanel;
    Button[] levelButtons;

    void Start()
    {
        menuPanel = transform.Find("MenuPanel").gameObject;
        levelButtons = menuPanel.GetComponentsInChildren<Button>();
        
        var curScene = SceneManager.GetActiveScene().buildIndex;
        for (int i = 0; i < levelButtons.Length; i++)
        {
            var button = levelButtons[i];
            if (i == curScene)
                button.GetComponentInChildren<Text>().color = current;
            
            int index = i;
            button.onClick.AddListener(() => 
            {
                GameManager.Instance.LoadLevel(index);
            });
        }

        if (GameManager.Instance.isTouchInput)
        {
            transform.Find("RestartButton").gameObject.SetActive(true);
        }
    }

    public void ToggleMenuPanel()
    {
        if (menuPanel.activeInHierarchy)
        {
            menuPanel.SetActive(false);
            GetComponent<Image>().raycastTarget = false;
        }
        else
        {
            menuPanel.SetActive(true);
            GetComponent<Image>().raycastTarget = true;
        }
    }

    public void Restart()
    {
        GameManager.Instance.Restart();
    }


}
