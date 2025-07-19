using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ThirstPlayer : MonoBehaviour
{
    private Rigidbody2D rb;
    private float verticalInput;
    private float speed = 8f;
    private float jumpHeight = 10f;
    private bool onGround = true;

    [SerializeField] private DataObject dataObject;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        verticalInput = Input.GetAxisRaw("Horizontal");

        if (rb)
        {
            rb.linearVelocity = new Vector2(verticalInput * speed, rb.linearVelocity.y);
        }


    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space) && onGround)
        {
            Jump();
        }
    }

    void Jump()
    {
        Debug.Log("jmp");
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpHeight);
        onGround = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            onGround = true;
        }
    }
    
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "WaterDrop")
        {
            WaterDrop wd = collider.GetComponent<WaterDrop>();
            if (wd == null) return;

            dataObject.PlayerData.GameData.UpdateStat("Thirst", wd.Point);
            Destroy(collider.gameObject, 0.01f);
        }
    }
}
