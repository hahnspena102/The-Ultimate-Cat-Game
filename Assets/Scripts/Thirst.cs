using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Thirst : MonoBehaviour
{
    [Header("Basics")]
    [SerializeField] private DataObject dataObject;
    [SerializeField] private GameObject waterDrop;

    [Header("Values")]
    [SerializeField] private Values values;
    [SerializeField] private float timeMax = 0.5f;
    [SerializeField] private float spawnWidth = 8f;
    
    //[Header("Audio Clips & Sprites")]

    

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
            wd.Point = Random.Range(values.WaterDropLowerBound, values.WaterDropUpperBound + 1);

            // Aquatic Alchemy 1
            if (dataObject.PlayerData.GameData.Upgrades[22])
            {
                int randomInt = Random.Range(0, 10);
                if (randomInt == 0)
                {
                    wd.Point = Mathf.Abs(wd.Point);
                    wd.IsGold = true;
                }
            }
                
            
        }

        Vector2 randomScreenPoint = new Vector2(
           Random.Range(0 - spawnWidth, 0 + spawnWidth + 1), 10
       );

        newDrop.transform.localPosition = randomScreenPoint;
    }
}
