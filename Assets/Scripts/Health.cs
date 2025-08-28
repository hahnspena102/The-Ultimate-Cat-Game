using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Health : MonoBehaviour
{
    [Header("Basics")]
    [SerializeField] private DataObject dataObject;
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private List<GameObject> boxes;

    [Header("Values")]
    [SerializeField] private Values values;
    [SerializeField] private int treatmentCount = 1;
    [SerializeField] private int currentTreatmentFound;

    [Header("Audio Clips & Sprites")]
    [SerializeField] private AudioClip randomizeSFX;
    [SerializeField] private AudioClip randomizeDoneSFX;


    private UI ui;
    private bool isResetting = false;
    public global::System.Int32 TreatmentCount { get => treatmentCount; set => treatmentCount = value; }
    public global::System.Int32 CurrentTreatmentFound { get => currentTreatmentFound; set => currentTreatmentFound = value; }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject go = GameObject.Find("UI");
        if (go) ui = go.GetComponent<UI>();

        int i = 0;
        int j = 0;
        foreach (GameObject box in boxes)
        {
            HealthBox hb = box.GetComponent<HealthBox>();
            if (hb == null) return;

            if (i >= 5)
            {
                i = 0;
                j += 1;
            }
            hb.Col = i++;
            hb.Row = j;
        }

        StartCoroutine(ResetGrid());
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTreatmentFound >= treatmentCount && !isResetting)
        {
            dataObject.PlayerData.GameData.UpdateStat("Health", values.HealthCompletionBonus);
            float pw = 16f;
            float ph = 16f;
            Vector2 spawnPosition = new Vector2(Random.Range(transform.position.x - pw, transform.position.x + pw),
                                                Random.Range(transform.position.y - ph, transform.position.y + ph));
            CreatePopup(spawnPosition, values.HealthCompletionBonus * treatmentCount);

            StartCoroutine(ResetGrid());

        }
    }

    public IEnumerator ResetGrid()
    {
        isResetting = true;
        currentTreatmentFound = 0;

        StartCoroutine(RevealAll());

        yield return new WaitForSeconds(1f);

        for (int i = 0; i < 3; i++)
        {
            RandomizeAll();
            yield return new WaitForSeconds(0.1f);
        }

        Util.PlaySFX(audioSource, randomizeDoneSFX);

        yield return new WaitForSeconds(1f);

        StartCoroutine(HideAll());
        isResetting = false;
    }

    IEnumerator HideAll()
    {
        foreach (GameObject box in boxes)
        {
            HealthBox hb = box.GetComponent<HealthBox>();
            if (hb == null) yield break;

            hb.Hide();
        }

        //yield return new WaitForSeconds(1f);

        foreach (GameObject box in boxes)
        {
            HealthBox hb = box.GetComponent<HealthBox>();
            if (hb == null) yield break;

            hb.IsOpen = false;
        }
    }

    IEnumerator RevealAll()
    {
        foreach (GameObject box in boxes)
        {
            HealthBox hb = box.GetComponent<HealthBox>();
            if (hb == null) yield break;

            hb.IsOpen = true;
        }

        foreach (GameObject box in boxes)
        {
            HealthBox hb = box.GetComponent<HealthBox>();
            if (hb == null) yield break;

            hb.Reveal();
        }
    }

    void RandomizeAll()
    {
        treatmentCount = 0;
        foreach (GameObject box in boxes)
        {
            HealthBox hb = box.GetComponent<HealthBox>();
            if (hb == null) return;

            Util.PlaySFX(audioSource, randomizeSFX);
            hb.Randomize();
        }
    }

    public void CreatePopup(Vector3 spawnPosition, int pointChange, bool isCoin = false)
    {
        if (isCoin)
        {
            if (ui) ui.SpawnCoinPopup(pointChange, transform);
        }
        else
        {
            if (ui) ui.SpawnPopup(spawnPosition, "Health", pointChange, transform);
        }
        
    }
}
