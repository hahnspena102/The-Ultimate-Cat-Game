using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private DataObject dataObject;
    [SerializeField] private float decayRate = 1;
    [SerializeField] private HungerTable hungerTable;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hungerTable.Initialize();
        StartCoroutine(GameCoroutine());

        StartCoroutine(AppetiteCoroutine());                                                          
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Coroutine that happens every second
    IEnumerator GameCoroutine()
    {
        List<Stat> stats = dataObject.PlayerData.GameData.Stats;
        foreach (Stat stat in stats)
        {
            stat.Value = Mathf.Max(stat.Value - (int)decayRate * Random.Range(1, 6), 0);
        }

        dataObject.PlayerData.GameData.EnergyRespawn.Update(1);

        dataObject.PlayerData.GameData.SoulBullets.Update(1);

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
}
