using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class EnergyPlayer : MonoBehaviour
{
    private Rigidbody2D rb;
    private float verticalInput, horizontalInput;
    [SerializeField]private float verticalSpeed = 10f;
    [SerializeField]private float horizontalSpeed = 10f;

    [SerializeField] private DataObject dataObject;

    private float flightHoldTime = 0f;
    private float flightExponentBase = 1.1f; // Base of exponential growth

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");

        if (rb)
        {
            rb.linearVelocity = new Vector2(horizontalInput * horizontalSpeed, rb.linearVelocity.y);
        }
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            flightHoldTime += Time.deltaTime;

            float exponentialBoost = Mathf.Pow(flightExponentBase, flightHoldTime) * 2f;
            float newYVelocity = Mathf.Min(exponentialBoost, verticalSpeed);

            rb.linearVelocity = new Vector2(rb.linearVelocity.x, newYVelocity);
        }
        else
        {
            // Reset the flight hold time when space is released
            flightHoldTime = 0f;
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Nightmare")
        {
            // Handle collision logic here
        }
    }
}
