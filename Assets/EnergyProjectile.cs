using UnityEngine;

public class EnergyProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 10f;          
    [SerializeField] private float waveAmplitude = 0f;     
    [SerializeField] private float waveFrequency = 0f;    

    private Rigidbody2D rb;
    private float startTime;
    private float initialY;

    void Start()
    {
        Destroy(gameObject, 10f);
        rb = GetComponent<Rigidbody2D>();

        startTime = Time.time;
        initialY = transform.position.y;
    }

    void Update()
    {
        float elapsed = Time.time - startTime;

        float newY = initialY + Mathf.Sin(elapsed * waveFrequency) * waveAmplitude;

        transform.position = new Vector2(transform.position.x - speed * Time.deltaTime, newY);

    }
}
