using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Clean : MonoBehaviour
{
    [Header("Basics")]
    [SerializeField] private DataObject dataObject;
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource audioSource;
    [Header("Values")]
    [SerializeField] private Values values;

    [Header("Audio Clips & Sprites")]
    [SerializeField] private AudioClip redLightSFX;
    [SerializeField] private AudioClip greenLightSFX, yellowLightSFX;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        dataObject.PlayerData.GameData.CleanTimer -= Time.deltaTime;

        if (dataObject.PlayerData.GameData.CleanTimer <= 0f)
        {
            // GREEN
            if (dataObject.PlayerData.GameData.CleanPhase == 0)
            {
                dataObject.PlayerData.GameData.CleanTimer = values.YellowLight;
                dataObject.PlayerData.GameData.CleanPhase = 1;
                if (audioSource) Util.PlaySFX(audioSource, yellowLightSFX);
            }
            // YELLOW
            else if (dataObject.PlayerData.GameData.CleanPhase == 1)
            {
                dataObject.PlayerData.GameData.CleanTimer = Random.Range(values.RedLightLower, values.RedLightUpper);
                dataObject.PlayerData.GameData.CleanPhase = 2;
                if (audioSource) Util.PlaySFX(audioSource, redLightSFX);
            }
            // RED
            else if (dataObject.PlayerData.GameData.CleanPhase == 2)
            {
                dataObject.PlayerData.GameData.CleanTimer = Random.Range(values.GreenLightLower, values.GreenLightUpper);
                dataObject.PlayerData.GameData.CleanPhase = 0;
                if (audioSource) Util.PlaySFX(audioSource, greenLightSFX);
                
            }
        }

        animator.SetInteger("phase", dataObject.PlayerData.GameData.CleanPhase);

    }

    public void PlaySFX()
    {
        audioSource.pitch = Random.Range(1.00f - 0.10f, 1.00f + 0.10f);
        audioSource.Play();
    }
}


