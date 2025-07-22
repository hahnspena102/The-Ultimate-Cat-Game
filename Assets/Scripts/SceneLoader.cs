using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private DataObject dataObject;
    private string currentContentScene = "CozyScene";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
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
            dataObject.CurrentStat = "Clean";
            SwitchToScene("CozyScene");
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
