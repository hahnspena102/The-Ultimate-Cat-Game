using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SoulGhost : MonoBehaviour
{
    [SerializeField] private DataObject dataObject;
    private Rigidbody2D rb;
    private float speed = 2f;

    private int health = 3;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 direction = new Vector2(dataObject.PlayerData.GameData.SoulX - transform.localPosition.x,
                                        dataObject.PlayerData.GameData.SoulY - transform.localPosition.y);

        direction = direction.normalized;
        rb.linearVelocity = direction * speed;

    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "SoulShot")
        {
            health -= 1;
            if (health <= 0)
            {
                dataObject.PlayerData.GameData.UpdateStat("Soul", 100);
                Destroy(gameObject);
            }
           
        }
    }
}
