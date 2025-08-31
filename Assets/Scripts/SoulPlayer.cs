using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SoulPlayer : MonoBehaviour
{
    [Header("Basics")]
    

    [SerializeField] private DataObject dataObject;
    [SerializeField] private GameObject soulShotPrefab;
    [SerializeField] private TMPro.TextMeshProUGUI textBox;
    [SerializeField] private Transform cursor;
    [SerializeField] private Camera targetCamera;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private AudioSource audioSource;
    private Rigidbody2D rb;
    [Header("Values")]
        [SerializeField] private Values values;
    [SerializeField, ReadOnly] private float verticalInput, horizontalInput;
    private float speed = 6f;
    private Soul soul;

    private float projectileSpeed = 10f;

    [SerializeField, ReadOnly] private bool isAttacking = false;

    [Header("Audio Clips & Sprites")]
    [SerializeField] private AudioClip hurtSFX;
    [SerializeField] private AudioClip shootSFX;

         private Color ogColor;
             private UI ui;

        


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
                soul = GameObject.FindFirstObjectByType<Soul>();
                ui = GameObject.FindFirstObjectByType<UI>();

        rb = GetComponent<Rigidbody2D>();
        ogColor = spriteRenderer.color;
        transform.localPosition = new Vector3(dataObject.PlayerData.GameData.SoulX, dataObject.PlayerData.GameData.SoulY, 1);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (rb)
        {
            rb.linearVelocity = new Vector2(horizontalInput * speed, verticalInput * speed);
        }


    }

    void Update()
    {
        if (dataObject)
        {
            dataObject.PlayerData.GameData.SoulX = transform.localPosition.x;
            dataObject.PlayerData.GameData.SoulY = transform.localPosition.y;

            if (textBox)
            {
                textBox.text = $"{dataObject.PlayerData.GameData.SoulBullets.Value}/{dataObject.PlayerData.GameData.SoulBullets.MaxValue}";
            }

        }

        if (cursor) UpdateCursor();

        if (Input.GetKeyDown(KeyCode.Mouse0) && dataObject.PlayerData.GameData.SoulBullets.Value > 0)
        {
            Shoot(1);
        }

        if (Input.GetKeyDown(KeyCode.Mouse1) && dataObject.PlayerData.GameData.SoulBullets.Value > 2)
        {
            Shoot(3);
        }

        if (rb != null && animator != null)
        {
            animator.SetFloat("vertical", rb.linearVelocity.y);
        }


        if (horizontalInput != 0 && !isAttacking)
        {
            transform.localScale = new Vector3(
                Mathf.Abs(transform.localScale.x) * Mathf.Sign(horizontalInput),
                transform.localScale.y,
                transform.localScale.z
            );
        }
    }

    void UpdateCursor()
    {
        if (targetCamera == null) return;
        Vector3 mouseScreen = Input.mousePosition;
        //mouseScreen.z = Mathf.Abs(targetCamera.transform.position.z);

        cursor.position = mouseScreen;
    }

    void Shoot(int numBullets)
    {
        float angleRange =20f;
        if (numBullets == 1)
        {
            CreateProj(0);
            return;
        }

        float angleGap = angleRange / (numBullets - 1);
        float startAngle = -angleRange / 2;

        for (int i = 0; i < numBullets; i++) {
            float curAngle = startAngle + (i * angleGap);
            CreateProj(curAngle);
        }
            
               Vector3 mouseScreen = Input.mousePosition;
        Vector2 mouseWorld = targetCamera.ScreenToWorldPoint(mouseScreen);
        Vector2 spawnPosition = transform.position;

        Vector2 direction = (mouseWorld - spawnPosition).normalized;

        transform.localScale = new Vector3(
                Mathf.Abs(transform.localScale.x) * Mathf.Sign(direction.x),
                transform.localScale.y,
                transform.localScale.z
        );
        animator.SetFloat("attackVertical", direction.y);
        animator.SetTrigger("attack");

        if (audioSource) Util.PlaySFX(audioSource, shootSFX);

    }

    void CreateProj(float angleOffset)
    {
        if (targetCamera == null) return;
        Vector3 mouseScreen = Input.mousePosition;

        mouseScreen.z = Mathf.Abs(targetCamera.transform.position.z);


        Vector2 mouseWorld = targetCamera.ScreenToWorldPoint(mouseScreen);

        Vector2 spawnPosition = transform.position;


        Vector2 direction = (mouseWorld - spawnPosition).normalized;
        direction = Quaternion.Euler(0, 0, angleOffset) * direction;

        GameObject projectile = Instantiate(soulShotPrefab, spawnPosition, Quaternion.identity, soul.transform);

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        projectile.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));


        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.linearVelocity = direction * projectileSpeed;
        }

        dataObject.PlayerData.GameData.SoulBullets.Update(-1);

        Debug.DrawLine(spawnPosition, mouseWorld, Color.red, 2f);
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ghost")
        {
            Hurt();
            soul.ClearField();
        }
    }

    void Hurt()
    {
        dataObject.PlayerData.GameData.UpdateStat("Soul", -1000);
         if (ui) ui.SpawnPopup(soul.Camera.WorldToScreenPoint(transform.position), "Soul", -1000, soul.Canvas.transform);

        if (audioSource) Util.PlaySFX(audioSource, hurtSFX);

        StartCoroutine(HurtEffect());
       

    }


    IEnumerator HurtEffect()
    {
        spriteRenderer.color = Color.red;

        float duration = 0.4f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
        elapsedTime += Time.deltaTime;
        spriteRenderer.color = Color.Lerp(Color.red, ogColor, elapsedTime / duration);
        yield return null;
        }

        spriteRenderer.color = ogColor;
    }

    public void SetIsAttacking(int i)
    {
        isAttacking = i != 0;
    }

    void OnDestroy()
    {
        dataObject.PlayerData.GameData.SoulX = transform.localPosition.x; 
        dataObject.PlayerData.GameData.SoulY = transform.localPosition.y;
    }
    
}
