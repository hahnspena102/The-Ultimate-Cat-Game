using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class EnergyPlayer : MonoBehaviour
{
    [Header("Basics")]
    [SerializeField] private DataObject dataObject;
    [SerializeField] private Energy energy;
    [SerializeField] private Canvas canvas;
    private Rigidbody2D rb;
    private UI ui;
    
    [Header("Values")]
    [SerializeField] private Values values;
    private float horizontalInput, verticalInput;
    

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
        rb.linearVelocity = new Vector2(horizontalInput * values.EnergyPlayerHorizontalSpeed, verticalInput * values.EnergyPlayerVerticalSpeed);
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
            Destroy(collider.gameObject);
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
