using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private DataObject dataObject;
    [SerializeField] private Values values;
    [SerializeField] private float decayRate = 1; // Occurrences per second
    [SerializeField] private HungerTable hungerTable;

    private float decayMultiplier = 0.12f; //The higher, the faster

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        hungerTable.Initialize();
        List<Stat> stats = dataObject.PlayerData.GameData.Stats;
        foreach (Stat stat in stats)
        {
            StartCoroutine(StatCoroutine(stat));
        }

        StartCoroutine(GameCoroutine());

        StartCoroutine(AppetiteCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        dataObject.PlayerData.GameData.UpdateLevel();
        UpdateStatLocks();
        UpdateValues();

        decayRate = Mathf.Max(1, dataObject.PlayerData.GameData.Level * decayMultiplier);

        foreach (Stat stat in dataObject.PlayerData.GameData.Stats)
        {
            if (stat.Value == 0)
            {
               Lose();
            }
        }
        
    }

    // Coroutine that updates at difficulty rate
    IEnumerator StatCoroutine(Stat stat)
    {
        while (dataObject.IsPaused || stat.Locked) yield return null;

        if ((float)stat.Value / (float)stat.MaxValue <= 0.05f)
        {
            stat.Update(-Random.Range(1, 6));

            yield return new WaitForSeconds(1f);
            StartCoroutine(StatCoroutine(stat));
        }
        else
        {
            stat.Update(-Random.Range(1, 6));

            yield return new WaitForSeconds(1 / decayRate);
            StartCoroutine(StatCoroutine(stat));
        }

    }

    // Coroutine that happens every second
    IEnumerator GameCoroutine()
    {
        while (dataObject.IsPaused) yield return null;

        dataObject.PlayerData.GameData.EnergyRespawn.Update(1);


        dataObject.PlayerData.GameData.UpdatePoints(100);
        //dataObject.PlayerData.GameData.Coins += 1;

        yield return new WaitForSeconds(1f);
        StartCoroutine(GameCoroutine());
    }

    IEnumerator AppetiteCoroutine()
    {
        dataObject.PlayerData.GameData.Appetite.Update(Mathf.FloorToInt(0.02f * dataObject.PlayerData.GameData.Appetite.MaxValue));
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(AppetiteCoroutine());
    }

    private List<int> levelThresholds = new List<int> { 0, 3, 6, 10, 15, 20, 30, 40, 50 };
    private void UpdateStatLocks()
    {
        List<Stat> stats = dataObject.PlayerData.GameData.Stats;
        for (int i = 0; i < stats.Count; i++)
        {
            stats[i].Locked = dataObject.PlayerData.GameData.Level < levelThresholds[i];
        }
    }

    public void CloseCallback()
    {
        Debug.Log("Browser tab closing — running OnApplicationQuit manually.");
        Application.Quit();
    }

    void OnApplicationQuit()
    {
        dataObject.PlayerData.SavePlayer();
    }

    void Lose()
    {
        SceneManager.LoadScene("GameOverScene");
    }

    void UpdateValues()
    {
        List<bool> upgrades = dataObject.PlayerData.GameData.Upgrades;

        values.UpdateUpgradeValues(upgrades);

        // Love
        dataObject.PlayerData.GameData.Stats[0].MaxValue = values.LoveMaxValue;

        // Hunger
        dataObject.PlayerData.GameData.Stats[1].MaxValue = values.HungerMaxValue;
        dataObject.PlayerData.GameData.Appetite.MaxValue = values.AppetiteMaxValue;

        // Thirst
        dataObject.PlayerData.GameData.Stats[2].MaxValue = values.ThirstMaxValue;

        // Energy
        dataObject.PlayerData.GameData.Stats[3].MaxValue = values.EnergyMaxValue;
        dataObject.PlayerData.GameData.EnergyRespawn.MaxValue = values.EnergyRespawnMaxValue;

        // Clean 
        dataObject.PlayerData.GameData.Stats[4].MaxValue = values.CleanMaxValue;

        // Cozy
        dataObject.PlayerData.GameData.Stats[5].MaxValue = values.CozyMaxValue;

        // Health
        dataObject.PlayerData.GameData.Stats[6].MaxValue = values.HealthMaxValue;

        // Soul
        dataObject.PlayerData.GameData.Stats[7].MaxValue = values.SoulMaxValue;
        dataObject.PlayerData.GameData.SoulBullets.MaxValue = values.MaxAmmo;
    
    }
}
