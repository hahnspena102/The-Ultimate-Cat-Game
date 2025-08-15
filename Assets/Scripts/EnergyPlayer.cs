using UnityEngine;

public class EnergyPlayer : MonoBehaviour
{
    [SerializeField] private DataObject dataObject;
    [SerializeField] private float horizontalSpeed = 6f;
    [SerializeField] private float verticalSpeed = 8f;
    [SerializeField] private Energy energy;
    [SerializeField] private Canvas canvas;

    private Rigidbody2D rb;
    private float horizontalInput, verticalInput;
    private UI ui;

    void Start()
    {
        GameObject go = GameObject.Find("UI");
        if (go) ui = go.GetComponent<UI>();

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
            energy.PlayAwakenSFX();
            AwakenPlayer();
        }
        else if (collider.CompareTag("EnergyParticle"))
        {
            EnergyParticle ep = collider.gameObject.GetComponent<EnergyParticle>();
            energy.CollectEnergy(ep.EnergyValue, collider.transform.position);
            Destroy(collider.gameObject, 0.1f);
        }
        else if (collider.CompareTag("Ground"))
        {
            energy.PlayAwakenSFX();
            AwakenPlayer();
        }
    }

    void AwakenPlayer()
    {
        dataObject.PlayerData.GameData.EnergyRespawn.Value = 0;
    }

}
