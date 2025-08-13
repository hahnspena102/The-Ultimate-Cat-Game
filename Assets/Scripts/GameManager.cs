using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private DataObject dataObject;
    [SerializeField] private float decayRate = 1; // Occurrences per second
    [SerializeField] private HungerTable hungerTable;

    private float decayMultiplier = 0.2f; //The higher, the faster

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

        dataObject.PlayerData.GameData.Points += 10;
        dataObject.PlayerData.GameData.Coins += 1;

        yield return new WaitForSeconds(1f);
        StartCoroutine(GameCoroutine());
    }

    IEnumerator AppetiteCoroutine()
    {
        dataObject.PlayerData.GameData.Appetite = Mathf.Min(dataObject.PlayerData.GameData.MaxAppetite,
                                                            dataObject.PlayerData.GameData.Appetite += 2);
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(AppetiteCoroutine());
    }

    private List<int> levelThresholds = new List<int> { 0, 2, 3, 5, 10, 100, 100, 100, 100 };
    private void UpdateStatLocks()
    {
        List<Stat> stats = dataObject.PlayerData.GameData.Stats;
        for (int i = 0; i < stats.Count; i++)
        {
            stats[i].Locked = dataObject.PlayerData.GameData.Level < levelThresholds[i];
        }
    }

    void OnApplicationQuit()
    {
        dataObject.PlayerData.SavePlayer();
    }

    void Lose()
    {
        SceneManager.LoadScene("GameOverScene");
    }

}
