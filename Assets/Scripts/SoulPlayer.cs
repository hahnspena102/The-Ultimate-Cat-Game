using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SoulPlayer : MonoBehaviour
{
    [SerializeField] private DataObject dataObject;
    [SerializeField] private GameObject soulShotPrefab;
    [SerializeField] private TMPro.TextMeshProUGUI textBox;
    [SerializeField] private Transform cursor;
    [SerializeField] private Camera targetCamera;
    private Rigidbody2D rb;
    private float verticalInput, horizontalInput;
    private float speed = 6f;

    private float projectileSpeed = 10f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
            Shoot();
        }
    }

    void UpdateCursor()
    {
        if (targetCamera == null) return;
        Vector3 mouseScreen = Input.mousePosition;
        mouseScreen.z = Mathf.Abs(targetCamera.transform.position.z);
        Vector3 mouseWorld = targetCamera.ScreenToWorldPoint(mouseScreen);
        mouseWorld.z = 0f;
        cursor.localPosition = mouseWorld;
    }

    void Shoot()
    {
        if (targetCamera == null) return;
        Vector3 mouseScreen = Input.mousePosition;

        mouseScreen.z = Mathf.Abs(targetCamera.transform.position.z);


        Vector2 mouseWorld = targetCamera.ScreenToWorldPoint(mouseScreen);

        Vector2 spawnPosition = transform.position;


        Vector2 direction = (mouseWorld - spawnPosition).normalized;

        GameObject projectile = Instantiate(soulShotPrefab, spawnPosition, Quaternion.identity);
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
            dataObject.PlayerData.GameData.UpdateStat("Soul", -1000);
            Destroy(collision.gameObject);
            Debug.Log("ouch");
        }
    }
    
}
