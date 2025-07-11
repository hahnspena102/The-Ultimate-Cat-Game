using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private DataObject dataObject;
    [SerializeField] private float decayRate = 1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(GameCoroutine());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator GameCoroutine()
    {
        List<Stat> stats = dataObject.PlayerData.GameData.Stats;
        foreach (Stat stat in stats) {
            stat.Value -= (int)decayRate * Random.Range(1,6);
        }
        yield return new WaitForSeconds(1f);
        StartCoroutine(GameCoroutine());
    }
}
