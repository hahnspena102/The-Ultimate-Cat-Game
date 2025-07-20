using UnityEngine;

public class EnergyPlayer : MonoBehaviour
{
    [SerializeField] private DataObject dataObject;
    [SerializeField] private float horizontalSpeed = 6f;
    [SerializeField] private float verticalSpeed = 8f;
    private Rigidbody2D rb;
    private float horizontalInput, verticalInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D not found on the player.");
        }
    }

    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(horizontalInput * horizontalSpeed, verticalInput * verticalSpeed);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("EnergyProjectile"))
        {
            AwakenPlayer();
        } else if (collider.CompareTag("Ground"))
        {
            AwakenPlayer();
        }
    }

    void AwakenPlayer()
    {
        dataObject.PlayerData.GameData.EnergyRespawn.Value = 0;
    }
}
