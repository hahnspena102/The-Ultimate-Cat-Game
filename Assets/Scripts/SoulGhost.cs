using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SoulGhost : MonoBehaviour
{
    [Header("Basics")]
    
    [SerializeField] private DataObject dataObject;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private AudioSource audioSource;
    private Rigidbody2D rb;
    [Header("Values")]
    [SerializeField] private Values values;

    private float speed = 2f;

    private int health = 3;
    private int maxHealth = 3;
    private Soul soul;

    private Color ogColor;
    private bool drops = true;
    [Header("Audio Clips & Sprites")]
    [SerializeField] private AudioClip hurtSFX;
    [SerializeField] private AudioClip deathSFX;
    public global::System.Int32 Health { get => health; set => health = value; }
    public global::System.Boolean Drops { get => drops; set => drops = value; }
    private UI ui;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        soul = GameObject.FindFirstObjectByType<Soul>();
        ui = GameObject.FindFirstObjectByType<UI>();
       // Debug.Log(ui);

        ogColor = spriteRenderer.color;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 direction = new Vector2(dataObject.PlayerData.GameData.SoulX - transform.localPosition.x,
                                        dataObject.PlayerData.GameData.SoulY - transform.localPosition.y);

        direction = direction.normalized;
        rb.linearVelocity = direction * speed;


        if (rb != null && animator != null)
        {
            animator.SetFloat("vertical", rb.linearVelocity.y);

            float baseScale = 1f;

            float scaleFactor = (float)health / 3f + 0.5f;

            transform.localScale = new Vector3(
                baseScale * scaleFactor * Mathf.Sign(rb.linearVelocity.x),
                baseScale * scaleFactor,
                transform.localScale.z
            );

        }

        if (health <= 0)
            {
                if (drops)
                {
                int pointChange = maxHealth * values.SoulValue;
                    dataObject.PlayerData.GameData.UpdateStat("Soul", pointChange);
                    if (ui) ui.SpawnPopup(soul.Camera.WorldToScreenPoint(transform.position), "Soul", pointChange, soul.Canvas.transform);

                }
                soul.RemoveGhost(gameObject);
                if (audioSource) Util.PlaySFX(audioSource, deathSFX);
                audioSource.transform.SetParent(transform.parent);
                Destroy(gameObject);
        }

    }
    void Hurt()
    {
        health -= 1;
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

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "SoulShot")
        {
            Hurt();
        

        }
    }

    public void LoadSoulGhost(SavedSoulGhost ssg)
    {
        transform.localPosition = new Vector3(ssg.XPos, ssg.YPos, 1);
        transform.localScale = new Vector3(ssg.XScale, transform.localScale.y, transform.localScale.z);
        health = ssg.Health;
        maxHealth = ssg.MaxHealth;
    }

    public SavedSoulGhost SaveSoulGhost()
    {
        return new SavedSoulGhost(transform.localPosition.x, transform.localPosition.y, transform.localScale.x, health, maxHealth);
    }

    public void SetHealth(int h)
    {
        maxHealth = h;
        health = h;
    }
    
    
}
