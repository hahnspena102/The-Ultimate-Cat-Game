using UnityEngine;
using System.Collections.Generic;

public class CozyBall : MonoBehaviour
{
    [SerializeField] private DataObject dataObject;
    [SerializeField] private string type;

    private HashSet<CozyBall> connectedBalls = new HashSet<CozyBall>();
    private bool checkedConnections = false;

    void Start()
    {
        // Optionally, check adjacent at start (e.g., if static)
    }

    void Update()
    {
        // Optional: Visualize or debug connections
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("CozyBall"))
        {
            CozyBall other = collision.gameObject.GetComponent<CozyBall>();
            if (!other || other.type != this.type) return;

            if (!checkedConnections)
            {
                connectedBalls = new HashSet<CozyBall>();
                FindConnectedBalls(this);
                if (connectedBalls.Count >= 3)
                {
                    foreach (var ball in connectedBalls)
                    {
                        Destroy(ball.gameObject, 0.01f);
                    }
                    dataObject.PlayerData.GameData.UpdateStat("Cozy", 10 * connectedBalls.Count);
                }
                checkedConnections = true;
            }
        }
    }

    void FindConnectedBalls(CozyBall origin)
    {
        if (connectedBalls.Contains(origin)) return;

        connectedBalls.Add(origin);

        Collider2D[] hits = Physics2D.OverlapCircleAll(origin.transform.position, 0.6f);
        foreach (var hit in hits)
        {
            CozyBall cb = hit.GetComponent<CozyBall>();
            if (cb && cb.type == origin.type && !connectedBalls.Contains(cb))
            {
                FindConnectedBalls(cb);
            }
        }
    }
}
