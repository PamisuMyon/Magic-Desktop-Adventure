using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour 
{
    public static LevelManager Instance { get; private set; }

    int gemsTotal;
    int gemsCount;
    Portal portal;

    Text gemsText;

    private void Awake() 
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }    
        Instance = this;

        var gems = GameObject.FindObjectsOfType<Gem>();    
        gemsTotal = gems.Length;

        portal = GameObject.FindGameObjectWithTag("Portal").GetComponent<Portal>();
        gemsText = GameObject.FindGameObjectWithTag("GemsText").GetComponent<Text>();

    }

    private void Start() 
    {
        UpdateGemsText();
    }

    public void GemCollected()
    {
        gemsCount++;
        if (gemsCount == gemsTotal)
        {
            portal.Show();
        }
        UpdateGemsText();
    }
    
    void UpdateGemsText()
    {
        gemsText.text = gemsCount + " / " + gemsTotal;
    }

}