using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Health : MonoBehaviour
{
    [SerializeField] private DataObject dataObject;

    [SerializeField] private List<GameObject> boxes;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(ShowcaseAll());
        RandomizeAll();
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator ShowcaseAll()
    {
        foreach (GameObject box in boxes)
        {
            HealthBox hb = box.GetComponent<HealthBox>();
            if (hb == null) yield break;

            hb.Reveal();
        }

        yield return new WaitForSeconds(2f);

        foreach (GameObject box in boxes)
        {
            HealthBox hb = box.GetComponent<HealthBox>();
            if (hb == null) yield break;

            hb.Hide();
        }
    }

    void RandomizeAll()
    {
        foreach (GameObject box in boxes)
        {
            HealthBox hb = box.GetComponent<HealthBox>();
            if (hb == null) return;

            hb.Randomize();
        }
    }
}
