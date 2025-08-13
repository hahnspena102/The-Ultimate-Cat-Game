using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private DataObject dataObject;
    private SceneLoader sceneLoader;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    void Awake()
    {

        sceneLoader = GameObject.FindFirstObjectByType<SceneLoader>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LeaveGame()
    {
        dataObject.PlayerData.SavePlayer();
        StartCoroutine(LeavingGame());
    }

    IEnumerator LeavingGame()
    {
        yield return new WaitForSeconds(0.1f);

        SceneManager.LoadScene("MainMenuScene");
    }

    public void Continue()
    {
        if (sceneLoader == null) return;
        sceneLoader.SwitchToPrevious();
    }
}
