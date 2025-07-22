using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private DataObject dataObject;
    [SerializeField] private Button newGameButton, continueButton, settingsButton;


    // Start is called before the first frame update
    void Awake()
    {
        dataObject.PlayerData.LoadPlayer();

        if (dataObject)
        {
            GameData gd = dataObject.PlayerData.GameData;
            if (gd.Points > 0)
            {
                if (continueButton) continueButton.interactable = true;
            }
            else
            {
                Debug.Log("no game data");
                dataObject.PlayerData.GameData = new GameData();
                if (continueButton) continueButton.interactable = false;
            }
        }
    }
    void Start()
    {
        Debug.Log(dataObject.PlayerData);
    }


    // Update is called once per frame
    void Update()
    {
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
    }
}
