using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour 
{
    public static GameManager Instance { get; private set; }

    public bool isTouchInput = false;

    [SerializeField] int currentScene;

    private void Awake() 
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }    
        Instance = this;

        DontDestroyOnLoad(this.gameObject);
    }

    private void Update() 
    {
        if (Input.GetButton("Restart"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Screen.fullScreen = false; 
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            if (Screen.fullScreen)
            {
                Screen.fullScreen = false;
            }
            else
            {
                Resolution[] resolutions = Screen.resolutions;
                Screen.SetResolution(resolutions[resolutions.Length - 1].width, resolutions[resolutions.Length - 1].height, true);
            }
        }
    }

    public void LoadLevel(int index)
    {
        SceneManager.LoadScene(currentScene = index);
    }

    public void NextLevel()
    {
        currentScene++;
        if (currentScene == SceneManager.sceneCountInBuildSettings)
        {
            EndUI.Instance.Show();
        }
        else
        {
            SceneManager.LoadScene(currentScene);
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(currentScene);
    }

    public void PlayAgain()
    {
        LoadLevel(0);
    }

}