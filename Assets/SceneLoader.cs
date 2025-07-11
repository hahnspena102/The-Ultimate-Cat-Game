using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private DataObject dataObject;
    private string currentContentScene = "";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchToScene("LoveScene");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchToScene("HungerScene");
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
