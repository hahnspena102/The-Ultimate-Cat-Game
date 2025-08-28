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
    [SerializeField] private TMP_InputField nameInputField;

    // Start is called before the first frame update
    void Awake()
    {
        dataObject.PlayerData.LoadPlayer();

        
        nameInputField.text = $"{dataObject.PlayerData.GameData.CatName}";
        if (!dataObject.PlayerData.GameData.IsAlive)
        {
            nameInputField.text = "";
        }
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
                //nameInputField.interactable = false;
            }
            else
            {
                //Debug.Log("no game data");
                dataObject.PlayerData.GameData = new GameData();
                if (continueButton) continueButton.interactable = false;
                //nameInputField.interactable = true;
            }
        }

        if (highscoreText) highscoreText.text = $"High Score: {dataObject.PlayerData.HighScore}";


        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.R))
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

        dataObject.PlayerData.GameData.CatName = nameInputField.text == "" ? "Meowser" : nameInputField.text;
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene("GameScene");
        SceneManager.LoadScene("LoveScene", LoadSceneMode.Additive);
    }
}
