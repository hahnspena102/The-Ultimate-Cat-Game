using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class HealthBox : MonoBehaviour
{
    [SerializeField] private CanvasGroup cover;

    [SerializeField] private string type;
    [SerializeField] private List<Sprite> treatmentSprites;
    [SerializeField] private List<Sprite> cathogenSprites;


    private float fadeDuration = 0.5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Reveal()
    {
        StartCoroutine(Fade(1f, 0f));
    }

    public void Hide()
    {
        StartCoroutine(Fade(0f, 1f));
    }

    IEnumerator Fade(float start, float end)
    {
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            float alpha;
            if (end > start)
            {
                alpha = start + (elapsed / fadeDuration);
            }
            else
            {
                alpha = start - (elapsed / fadeDuration);
            }


            if (cover != null)
            {
                cover.alpha = alpha;
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        if (cover != null) cover.alpha = end;

    }

    public void Randomize()
    {
        List<string> types = new List<string> { "treatment", "cathogen", "none" };
        List<float> odds = new List<float> { 20f, 20f, 60f };

        float totalWeight = 0f;
        foreach (float weight in odds)
        {
            totalWeight += weight;
        }

        float randomValue = Random.Range(0f, totalWeight);

        float cumulative = 0f;
        for (int j = 0; j < odds.Count; j++)
        {
            cumulative += odds[j];
            if (randomValue <= cumulative)
            {
                type = types[j];
                break;
            }
        }
        

    }
}
