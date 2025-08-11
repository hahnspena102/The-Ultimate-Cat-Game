using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ThirstPlayer : MonoBehaviour
{
    [SerializeField] private DataObject dataObject;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip collectSFX, hurtSFX;
    [SerializeField] private Canvas canvas;
    private Rigidbody2D rb;
    private float verticalInput;
    private float speed = 8f;
    private float jumpHeight = 10f;
    private bool onGround = true;
    private UI ui;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        GameObject go = GameObject.Find("UI");
        if (go) ui = go.GetComponent<UI>();
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

             if (audioSource)
            {
                if (wd.Point > 0)
                {
                    audioSource.clip = collectSFX;
                }
                else
                {
                    audioSource.clip = hurtSFX;
                }

                audioSource.pitch = Random.Range(1.00f - 0.10f, 1.00f + 0.10f);
                audioSource.Play();
            }

            //Debug.Log(Camera.main.WorldToScreenPoint(transform.position));

            if (ui) ui.SpawnPopup(Camera.main.WorldToScreenPoint(transform.position), "Thirst", wd.Point, canvas.transform);


            Destroy(collider.gameObject, 0.01f);
        }
    }
}
