using UnityEngine;

public class SoulShot : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void OnCollisionEnter2D()
    {
        Destroy(gameObject);
    }
}
