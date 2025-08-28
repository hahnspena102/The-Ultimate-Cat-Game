using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanPlayer : MonoBehaviour
{
    [Header("Basics")]
    
    [SerializeField] private DataObject dataObject;
    [SerializeField] private Canvas canvas;
    [SerializeField] private AudioSource audioSource;
    private UI ui;
    [Header("Values")]
    [SerializeField] private Values values;
    [SerializeField] private float boostDecayRate;
    private Rigidbody2D rb;
    private float currentBoost = 0f;
    private Vector2 originalPos;

    [Header("Audio Clips & Sprites")]
    [SerializeField] private AudioClip caughtSFX;
    [SerializeField] private AudioClip successSFX;
  

    void Awake()
    {
        originalPos = transform.localPosition;
        transform.localPosition = new Vector2(dataObject.PlayerData.GameData.CleanX, transform.localPosition.y);
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D not found on the player.");
        }

        GameObject go = GameObject.Find("UI");
        if (go) ui = go.GetComponent<UI>();
    }

    void Update()
    {
        dataObject.PlayerData.GameData.CleanX = transform.localPosition.x;
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.W))
        {
            if (dataObject.PlayerData.GameData.CleanPhase == 2)
            {
                

                // Trusting Temperament
                if (dataObject.PlayerData.GameData.Upgrades[41])
                {
                    transform.localPosition = new Vector2(Mathf.Max(transform.localPosition.x - 0.5f, originalPos.x), transform.localPosition.y);
                }
                else
                {
                    transform.localPosition = originalPos;
                }
                if (audioSource)
                {
                    audioSource.clip = caughtSFX;
                    audioSource.Play();
                }

            }
            else
            {
                currentBoost = values.CleanPlayerBoost;
            }

        }
    }

    void FixedUpdate()
    {
        if (rb)
        {
            rb.linearVelocity = new Vector2(currentBoost, rb.linearVelocity.y);
            currentBoost = Mathf.Lerp(currentBoost, 0f, boostDecayRate * Time.fixedDeltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("CleanCat"))
        {
            CleanCat();
        }

    }

    void CleanCat()
    {
        dataObject.PlayerData.GameData.UpdateStat("Clean", values.CleanValue);

        dataObject.PlayerData.GameData.UpdatePoints(values.CleanValue);

        if (ui) ui.SpawnPopup(Camera.main.WorldToScreenPoint(transform.position), "Clean", values.CleanValue, canvas.transform);
        if (audioSource)
        {
            audioSource.clip = successSFX;
            audioSource.Play();
        }

        transform.localPosition = originalPos;


    }
}
