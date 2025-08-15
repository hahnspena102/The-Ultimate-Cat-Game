using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private DataObject dataObject;
    [SerializeField] private Button newGameButton, continueButton, settingsButton;
    [SerializeField] private TextMeshProUGUI highscoreText;

    // Start is called before the first frame update
    void Awake()
    {
        dataObject.PlayerData.LoadPlayer();
    }


    // Update is called once per frame
    void Update()
    {
        if (dataObject)
        {
            GameData gd = dataObject.PlayerData.GameData;
            if (gd.IsAlive)
            {
                if (continueButton) continueButton.interactable = true;
            }
            else
            {
                //Debug.Log("no game data");
                dataObject.PlayerData.GameData = new GameData();
                if (continueButton) continueButton.interactable = false;
            }
        }

        if (highscoreText) highscoreText.text = $"High Score: {dataObject.PlayerData.HighScore}";



        if (Input.GetKeyDown(KeyCode.S))
        {
            dataObject.PlayerData.SavePlayer();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            dataObject.PlayerData.LoadPlayer();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log(dataObject.PlayerData);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Resetting Player Data");
            dataObject.PlayerData = new PlayerData();
            dataObject.PlayerData.SavePlayer();
        }
    }

    public void NewGame()
    {
        ResetGameData();
        StartCoroutine(LaunchingGame());
    }

    public void ContinueGame()
    {
        StartCoroutine(LaunchingGame());
    }

    public void ResetGameData()
    {
        Debug.Log("Resetting Game Data");
        dataObject.PlayerData.GameData = new GameData();
        dataObject.PlayerData.SavePlayer();
    }

    IEnumerator LaunchingGame()
    {
        dataObject.CurrentStat = "Love";
        dataObject.SelectedUpgrade = null;
        dataObject.PlayerData.GameData.IsAlive = true;
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene("GameScene");
        SceneManager.LoadScene("LoveScene", LoadSceneMode.Additive);
    }
}
