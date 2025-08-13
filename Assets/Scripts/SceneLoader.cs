using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private DataObject dataObject;
    [SerializeField] private CanvasGroup uiCanvasGroup;
    private string currentContentScene = "";
    private Dictionary<KeyCode, (string stat, string scene)> keyMappings;
    private string previousStat;

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

        keyMappings = new Dictionary<KeyCode, (string, string)>
        {
            { KeyCode.Alpha1, ("Love", "LoveScene") },
            { KeyCode.Alpha2, ("Hunger", "HungerScene") },
            { KeyCode.Alpha3, ("Thirst", "ThirstScene") },
            { KeyCode.Alpha4, ("Energy", "EnergyScene") },
            { KeyCode.Alpha5, ("Clean", "CleanScene") },
            { KeyCode.Alpha6, ("Cozy", "CozyScene") },
            { KeyCode.Alpha7, ("Health", "HealthScene") },
            { KeyCode.Alpha8, ("Soul", "SoulScene") },
            { KeyCode.Alpha9, ("Lifeforce", "LifeforceScene") },
            { KeyCode.U, (null, "UpgradeScene") }, // No stat change
            { KeyCode.Escape, (null, "PauseScene") } // No stat change
        };
    }

    // Update is called once per frame
    void Update()
    {
        if (currentContentScene != "UpgradeScene" && currentContentScene != "PauseScene")
        {
            uiCanvasGroup.alpha = 1f;
            dataObject.IsPaused = false;
        }
        else
        {
            uiCanvasGroup.alpha = 0f;
            dataObject.IsPaused = true;
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            FlipToStat(-1);
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            FlipToStat(1);
        }




        foreach (var entry in keyMappings)
        {
            if (Input.GetKeyDown(entry.Key))
            {
                if (!string.IsNullOrEmpty(entry.Value.stat))
                    if (dataObject.PlayerData.GameData.GetIsLocked(entry.Value.stat))
                    {
                        break;
                    }
                if (dataObject.CurrentStat != null) previousStat = dataObject.CurrentStat;
                dataObject.CurrentStat = entry.Value.stat;

                SwitchToScene(entry.Value.scene);
            }
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

    public void SwitchToStat(string stat)
    {
        if (dataObject.PlayerData.GameData.GetIsLocked(stat)) return;
        dataObject.CurrentStat = stat;
        SwitchToScene($"{stat}Scene");
    }


    public void FlipToStat(int delta)
    {
        List<string> scenesList = new List<string>
        {
            "Love", "Hunger", "Thirst", "Energy", "Clean","Cozy", "Health", "Soul", "Lifeforce"
        };


        int currentIndex = scenesList.IndexOf(dataObject.CurrentStat);
        currentIndex += delta;
        int newIndex = (currentIndex + scenesList.Count) % scenesList.Count;

        SwitchToStat(scenesList[newIndex]);
    }

    public void SwitchToPrevious()
    {
        Debug.Log(previousStat);
        if (previousStat == null) SwitchToStat("Love");
        SwitchToStat(previousStat);
    }

}
