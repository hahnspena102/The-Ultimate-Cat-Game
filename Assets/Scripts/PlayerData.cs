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

    // A unique, constant folder name to avoid breaking saves after updates
    private const string SAVE_FOLDER = "idbfs/The_Ultimate_Cat_Game"; // change this to your game's unique name
    private const string SAVE_FILE = "player.cat";

    public PlayerData()
    {
        username = "user";
        highScore = 0;
        deathCount = 0;
        gameData = new GameData();
    }

    // Get the correct save path for both WebGL and non-WebGL builds
    private static string GetSavePath()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        // Use a fixed idbfs directory to avoid losing saves on update
        return Path.Combine(SAVE_FOLDER, SAVE_FILE);
#else
        // Normal persistentDataPath for standalone builds
        return Path.Combine(Application.persistentDataPath, SAVE_FILE);
#endif
    }

    public void SavePlayer()
    {
        string path = GetSavePath();

        // Ensure directory exists (especially important for WebGL idbfs)
        Directory.CreateDirectory(Path.GetDirectoryName(path));

        BinaryFormatter formatter = new BinaryFormatter();
        using (Stream stream = File.Open(path, FileMode.Create))
        {
            try
            {
                formatter.Serialize(stream, this);
                Debug.Log($"Successfully saved player to: {path}");
            }
            catch (SerializationException e)
            {
                Debug.LogError("Failed to serialize. Reason: " + e.Message);
                throw;
            }
        }

#if UNITY_WEBGL && !UNITY_EDITOR
        // This is critical — persist IndexedDB changes
        PlayerPrefs.Save();
        Application.ExternalEval("_JS_FileSystem_Sync();"); 
#endif
    }

    public void LoadPlayer()
    {
        string path = GetSavePath();

        if (!File.Exists(path))
        {
            Debug.LogWarning("Save file not found at: " + path);
            return;
        }

        BinaryFormatter formatter = new BinaryFormatter();
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

    public string Username { get => username; set => username = value; }
    public int HighScore { get => highScore; set => highScore = value; }
    public int DeathCount { get => deathCount; set => deathCount = value; }
    public GameData GameData { get => gameData; set => gameData = value; }
}
