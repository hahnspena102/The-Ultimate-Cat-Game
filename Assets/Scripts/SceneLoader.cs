using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private DataObject dataObject;
    [SerializeField] private CanvasGroup uiCanvasGroup;
    private string currentContentScene = "";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.name != "GameScene")
            {
                currentContentScene = scene.name;
            }
           
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentContentScene != "UpgradeScene")
        {
            uiCanvasGroup.alpha = 1f;
            dataObject.IsPaused = false;
        }
        else
        {
            uiCanvasGroup.alpha = 0f;
            dataObject.IsPaused = true;
        }


        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            dataObject.CurrentStat = "Love";
            SwitchToScene("LoveScene");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            dataObject.CurrentStat = "Hunger";
            SwitchToScene("HungerScene");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            dataObject.CurrentStat = "Thirst";
            SwitchToScene("ThirstScene");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            dataObject.CurrentStat = "Energy";
            SwitchToScene("EnergyScene");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            dataObject.CurrentStat = "Clean";
            SwitchToScene("CleanScene");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            dataObject.CurrentStat = "Cozy";
            SwitchToScene("CozyScene");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            dataObject.CurrentStat = "Health";
            SwitchToScene("HealthScene");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            dataObject.CurrentStat = "Soul";
            SwitchToScene("SoulScene");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            dataObject.CurrentStat = "Lifeforce";
            SwitchToScene("LifeforceScene");
        }
        else if (Input.GetKeyDown(KeyCode.U))
        {
            SwitchToScene("UpgradeScene");
        }

    }

    void SwitchToScene(string sceneName)
    {
        if (currentContentScene == sceneName) 
            return;

        if (!string.IsNullOrEmpty(currentContentScene) && SceneManager.GetSceneByName(currentContentScene).isLoaded)
        {
            SceneManager.UnloadSceneAsync(currentContentScene);
        }

        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        currentContentScene = sceneName;
    }
}
