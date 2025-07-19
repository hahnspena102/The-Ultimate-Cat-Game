using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Thirst : MonoBehaviour
{
    [SerializeField] private GameObject waterDrop;
    [SerializeField] private int lowerBound = -100;
    [SerializeField] private int upperBound = 100;
    [SerializeField] private float timeMax = 0.5f;

    private float spawnWidth = 8f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(WaterGenerator());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator WaterGenerator()
    {
        GenerateDrop();
        yield return new WaitForSeconds(Random.Range(0.1f, timeMax));
        StartCoroutine(WaterGenerator());
    }

    void GenerateDrop()
    {
        if (waterDrop == null) return;
        GameObject newDrop = Instantiate(waterDrop, transform.position, Quaternion.identity);
        newDrop.transform.SetParent(transform);

        if (newDrop == null) return;
        WaterDrop wd = newDrop.GetComponent<WaterDrop>();

        if (wd)
        {
            wd.Point = Random.Range(lowerBound, upperBound + 1);
        }

        Vector2 randomScreenPoint = new Vector2(
           Random.Range(0 - spawnWidth, 0 + spawnWidth + 1), 10
       );

        newDrop.transform.localPosition = randomScreenPoint;
    }
}
