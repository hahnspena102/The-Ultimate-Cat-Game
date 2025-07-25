using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Soul : MonoBehaviour
{
    [SerializeField] private DataObject dataObject;
    [SerializeField] private GameObject ghostPrefab;
    private float ellipseWidth = 40f; 
    private float ellipseHeight = 20f;
    private float timerMin = 1f;
    private float timerMax = 5f;

    void Start()
    {
        StartCoroutine(SpawnCoroutine());
        
    }

    IEnumerator SpawnCoroutine()
    {
        SpawnGhost();
        yield return new WaitForSeconds(Random.Range(timerMin, timerMax));
        StartCoroutine(SpawnCoroutine());
    }

    void SpawnGhost()
    {
        float a = ellipseWidth / 2f;
        float b = ellipseHeight / 2f;

        float angle = Random.Range(0f, 2f * Mathf.PI);
        float x = a * Mathf.Cos(angle);
        float y = b * Mathf.Sin(angle);

        Vector3 spawnPos = transform.position + new Vector3(x, y, 0f);
        GameObject newGhost = Instantiate(ghostPrefab, spawnPos, Quaternion.identity);
        newGhost.transform.SetParent(transform);
    }
}
