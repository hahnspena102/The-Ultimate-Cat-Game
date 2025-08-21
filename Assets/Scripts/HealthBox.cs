using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class HealthBox : MonoBehaviour
{
    [SerializeField] private DataObject dataObject;
    [SerializeField] private CanvasGroup cover;

    [SerializeField] private string type;
    [SerializeField] private List<Sprite> treatmentSprites;
    [SerializeField] private List<Sprite> cathogenSprites;
    [SerializeField] private Sprite nullSprite;

    [SerializeField] private Image image;

    [SerializeField] private int row, col;
    [SerializeField] private bool isOpen;

    [SerializeField] private Button button;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip treatmentSFX, cathogenSFX, nullSFX;

    private Health health;

    private float fadeDuration = 0.65f;

    public global::System.Int32 Row { get => row; set => row = value; }
    public global::System.Int32 Col { get => col; set => col = value; }
    public global::System.Boolean IsOpen { get => isOpen; set => isOpen = value; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject go = transform.parent.transform.parent.gameObject;
        if (go)
        {
            health = go.GetComponent<Health>();     
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        button.interactable = !isOpen;
    }

    public void OpenBox()
    {
        if (isOpen) return;

        isOpen = true;
        Reveal();

        int pointChange = 0;
        if (type == "treatment")
        {
            pointChange = health.HealValue;
            Util.PlaySFX(audioSource, treatmentSFX, 0.2f);
            health.CurrentTreatmentFound += 1;
        }
        else if (type == "cathogen")
        {
            Util.PlaySFX(audioSource, cathogenSFX);
            pointChange = health.HurtValue;
            StartCoroutine(health.ResetGrid());
        }
        else
        {
            Util.PlaySFX(audioSource, nullSFX);
            StartCoroutine(health.ResetGrid());
        }

        if (pointChange != 0)
        {
            dataObject.PlayerData.GameData.UpdateStat("Health", pointChange);
            dataObject.PlayerData.GameData.UpdatePoints(pointChange);
            
            health.CreatePopup(transform.position, Mathf.FloorToInt(pointChange));
        }
        
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
        List<float> odds = health.Odds;

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

        if (image == null) return;


        if (type == "treatment")
        {
            image.sprite = treatmentSprites[Random.Range(0, treatmentSprites.Count)];
            health.TreatmentCount += 1;
        }
        else if (type == "cathogen")
        {
            image.sprite = cathogenSprites[Random.Range(0, cathogenSprites.Count)];
        }
        else
        {
            image.sprite = nullSprite;
        }
    }
}
