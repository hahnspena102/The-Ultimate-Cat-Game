using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [SerializeField] private DataObject dataObject;
    [SerializeField] private TextMeshProUGUI scoreText, highscoreText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dataObject.PlayerData.GameData.IsAlive = false;

        dataObject.PlayerData.HighScore = Mathf.Max(dataObject.PlayerData.HighScore, dataObject.PlayerData.GameData.Points);

        dataObject.PlayerData.SavePlayer();
    }
    

    // Update is called once per frame
    void Update()
    {
        if (dataObject == null) return;

        if (scoreText) scoreText.text = $"Score: {dataObject.PlayerData.GameData.Points}";
        if (highscoreText) highscoreText.text = $"High Score: {dataObject.PlayerData.HighScore}";
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
    
}
