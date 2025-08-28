using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ThirstPlayer : MonoBehaviour
{
    [Header("Basics")]
    
    [SerializeField] private DataObject dataObject;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Canvas canvas;

    private Rigidbody2D rb;
    private float verticalInput;
    private UI ui;
    private Animator animator;

    [Header("Values")]
    [SerializeField] private Values values;
    [SerializeField]private float jumpHeight = 10f;
    private bool onGround = true;


    [Header("Audio Clips & Sprites")]
    [SerializeField] private AudioClip collectSFX;
    [SerializeField] private AudioClip hurtSFX;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        GameObject go = GameObject.Find("UI");
        if (go) ui = go.GetComponent<UI>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        verticalInput = Input.GetAxisRaw("Horizontal");

        if (rb)
        {
            rb.linearVelocity = new Vector2(verticalInput * values.ThirstPlayerSpeed, rb.linearVelocity.y);
        }
        animator.SetFloat("movement", Mathf.Abs(rb.linearVelocity.magnitude));
        animator.SetFloat("vertical", rb.linearVelocity.y);


        if (rb.linearVelocity.x > 0.1f)
        {
            Vector3 currentScale = transform.localScale;
            currentScale.x = Mathf.Abs(currentScale.x);
            transform.localScale = currentScale;
        }
        else if (rb.linearVelocity.x < -0.1f)
        {
            Vector3 currentScale = transform.localScale;
            currentScale.x = -Mathf.Abs(currentScale.x);
            transform.localScale = currentScale;
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
            dataObject.PlayerData.GameData.UpdatePoints(wd.Point);
            if (wd.IsGold)
            {
                int coinChange = wd.Point;
                dataObject.PlayerData.GameData.Coins += coinChange;
                if (ui) ui.SpawnCoinPopup(coinChange, canvas.transform);


                dataObject.PlayerData.GameData.Coins += coinChange;
            }

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

                Util.PlaySFX(audioSource);
            }

            //Debug.Log(Camera.main.WorldToScreenPoint(transform.position));

            if (ui) ui.SpawnPopup(Camera.main.WorldToScreenPoint(transform.position), "Thirst", wd.Point, canvas.transform);


            Destroy(collider.gameObject, 0.01f);
        }
    }
}
