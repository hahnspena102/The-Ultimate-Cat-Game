using UnityEngine;

public class Clean : MonoBehaviour
{
    [SerializeField] private DataObject dataObject;
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip redLightSFX, greenLightSFX, yellowLightSFX;

    private float greenLower = 1f;
    private float greenUpper = 8f;

    private float redLower = 1f;
    private float redUpper = 3f;


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
                dataObject.PlayerData.GameData.CleanTimer = 1f;
                dataObject.PlayerData.GameData.CleanPhase = 1;
                if (audioSource)
                {
                    audioSource.clip = yellowLightSFX;
                    PlaySFX();
                }
            }
            // YELLOW
            else if (dataObject.PlayerData.GameData.CleanPhase == 1)
            {
                dataObject.PlayerData.GameData.CleanTimer = Random.Range(redLower, redUpper);
                dataObject.PlayerData.GameData.CleanPhase = 2;
                if (audioSource)
                {
                    audioSource.clip = redLightSFX;
                    PlaySFX();
                }
            }
            // RED
            else if (dataObject.PlayerData.GameData.CleanPhase == 2)
            {
                dataObject.PlayerData.GameData.CleanTimer = Random.Range(greenLower, greenUpper);
                dataObject.PlayerData.GameData.CleanPhase = 0;
                if (audioSource)
                {
                    audioSource.clip = greenLightSFX;
                    PlaySFX();
                }
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


