using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using UnityEngine;


[System.Serializable]
public class PlayerData
{
    [SerializeField] private string username;
    [SerializeField] private int highScore;
    [SerializeField] private int deathCount;
    [SerializeField] private GameData gameData;

    public PlayerData()
    {
        username = "user";
        highScore = 123;
        deathCount = 0;
        gameData = new GameData();
    }

    public void SavePlayer()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.cat";

        using (Stream stream = File.Open(path, FileMode.Create))
        {
            try
            {
                formatter.Serialize(stream, this);
                Debug.Log("Successfully saved player.");
            }
            catch (SerializationException e)
            {
                Debug.LogError("Failed to serialize. Reason: " + e.Message);
                throw;
            }
        }
    }

    public void LoadPlayer()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.cat";

        if (!File.Exists(path))
        {
            Debug.LogError("Save file not found in" + path);
        }

        using (Stream stream = File.Open(path, FileMode.Open))
        {
            try
            {
                PlayerData newPlayer = formatter.Deserialize(stream) as PlayerData;
                SetPlayer(newPlayer);
                Debug.Log("Successfully loaded player.");
            }
            catch (SerializationException e)
            {
                Debug.LogError("Failed to deserialize. Reason: " + e.Message);
                throw;
            }
        }
    }

    public void SetPlayer(PlayerData newPlayer)
    {
        username = newPlayer.username;
        highScore = newPlayer.highScore;
        deathCount = newPlayer.deathCount;
        gameData = newPlayer.gameData;
    }

    public override string ToString()
    {
        return $"PlayerData(\n" +
            $"  Username: {username},\n" +
            $"  HighScore: {highScore},\n" +
            $"  DeathCount: {deathCount},\n" +
            $"  GameData: {(gameData != null ? gameData.ToString() : "null")}\n)";
    }

    public global::System.String Username { get => username; set => username = value; }
    public global::System.Int32 HighScore { get => highScore; set => highScore = value; }
    public global::System.Int32 DeathCount { get => deathCount; set => deathCount = value; }
    public GameData GameData { get => gameData; set => gameData = value; }
}
