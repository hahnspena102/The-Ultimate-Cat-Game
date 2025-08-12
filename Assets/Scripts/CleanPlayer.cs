using UnityEngine;

public class CleanPlayer : MonoBehaviour
{
    [SerializeField] private DataObject dataObject;
    [SerializeField] private float boostSpeed;
    [SerializeField] private float boostDecayRate;
    [SerializeField] private Canvas canvas;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip caughtSFX, successSFX;
    private Rigidbody2D rb;
    private float currentBoost = 0f;
    private Vector2 originalPos;
    private UI ui;
    private int cleanValue = 500;

    void Start()
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (dataObject.PlayerData.GameData.CleanPhase == 2)
            {
                transform.localPosition = originalPos;
                if (audioSource)
                {
                    audioSource.clip = caughtSFX;
                    audioSource.Play();
                }
        
            }
            else
            {
                currentBoost = boostSpeed;
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
        dataObject.PlayerData.GameData.UpdateStat("Clean", cleanValue);
        dataObject.PlayerData.GameData.Points += Mathf.Max(cleanValue, 0);

        if (ui) ui.SpawnPopup(Camera.main.WorldToScreenPoint(transform.position), "Clean", cleanValue, canvas.transform);
        if (audioSource)
        {
            audioSource.clip = successSFX;
            audioSource.Play();
        }

        transform.localPosition = originalPos;


    }
    

}
