using UnityEngine;

public class CleanPlayer : MonoBehaviour
{
    [SerializeField] private DataObject dataObject;
    [SerializeField] private float boostSpeed;
    [SerializeField] private float boostDecayRate;
    private Rigidbody2D rb;
    private float currentBoost = 0f;
    private Vector2 originalPos;

    void Start()
    {
        originalPos = transform.localPosition;
        transform.localPosition = new Vector2(dataObject.PlayerData.GameData.CleanX, transform.localPosition.y);
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D not found on the player.");
        }
    }

    void Update()
    {
        dataObject.PlayerData.GameData.CleanX = transform.localPosition.x;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (dataObject.PlayerData.GameData.CleanPhase == 2)
            {
                transform.localPosition = originalPos;
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
        transform.localPosition = originalPos;

        dataObject.PlayerData.GameData.UpdateStat("Clean", 500);
    }
    


}
