using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private DataObject dataObject;
    [SerializeField] private float decayRate = 1; // Occurrences per second
    [SerializeField] private HungerTable hungerTable;

    private float decayMultiplier = 0.1f; //The higher, the faster

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
        UpdateUpgradeValues();

        decayRate = Mathf.Max(1, dataObject.PlayerData.GameData.Level * decayMultiplier);

        foreach (Stat stat in dataObject.PlayerData.GameData.Stats)
        {
            if (stat.Value == 0)
            {
               // Lose();
            }
        }
        
    }

    // Coroutine that updates at difficulty rate
    IEnumerator StatCoroutine(Stat stat)
    {
        while (dataObject.IsPaused || stat.Locked) yield return null;

        if ((float)stat.Value / (float)stat.MaxValue <= 0.05f)
        {
            stat.Update(-1);

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

        dataObject.PlayerData.GameData.SoulBullets.Update(1);

        dataObject.PlayerData.GameData.UpdatePoints(10);
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

    private List<int> levelThresholds = new List<int> { 0, 2, 3, 5, 10, 20, 30, 1000, 1000 };
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

    void UpdateUpgradeValues()
    {
        List<bool> upgrades = dataObject.PlayerData.GameData.Upgrades;

        // Love
        Stat love = dataObject.PlayerData.GameData.Stats[0];
        if (upgrades[2])
        {
            love.MaxValue = 2000;
        }
        else if (upgrades[1])
        {
            love.MaxValue = 1500;
        }
        else if (upgrades[0])
        {
            love.MaxValue = 1000;
        }
        else
        {
            love.MaxValue = 750;
        }

        // Hunger
        Stat hunger = dataObject.PlayerData.GameData.Stats[1];
        if (upgrades[13])
        {
            hunger.MaxValue = 9000;
            dataObject.PlayerData.GameData.Appetite.MaxValue = 300;
        }
        else if (upgrades[12])
        {
            hunger.MaxValue = 3000;
            dataObject.PlayerData.GameData.Appetite.MaxValue = 200;
        }
        else
        {
            hunger.MaxValue = 1000;
            dataObject.PlayerData.GameData.Appetite.MaxValue = 100;
        }


        // Thirst
        Stat thirst = dataObject.PlayerData.GameData.Stats[2];
        if (upgrades[20])
        {
            thirst.MaxValue = 4800;
        }
        else if (upgrades[19])
        {
            thirst.MaxValue = 2400;
        }
        else if (upgrades[18])
        {
            thirst.MaxValue = 1200;
        }
        else
        {
            thirst.MaxValue = 600;
        }

        // Clean
        Stat clean = dataObject.PlayerData.GameData.Stats[4];
        if (upgrades[39])
        {
            clean.MaxValue = 4000;
        }
        else
        {
             clean.MaxValue = 2000;
        }
    }
}
