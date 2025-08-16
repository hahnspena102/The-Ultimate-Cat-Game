using UnityEngine;
using System.Collections.Generic;

public class CozyBall : MonoBehaviour
{
    [SerializeField] private DataObject dataObject;
    [SerializeField] private string type;

    private HashSet<CozyBall> connectedBalls = new HashSet<CozyBall>();
    private bool checkedConnections = false;
    private Cozy cozyOwner;
    private UI ui;

    public global::System.String Type { get => type; set => type = value; }

    private Dictionary<string, int> labelToIndex = new Dictionary<string, int>
    {
        { "warmth", 0 },
        { "bed", 1 },
        { "tower", 2 },
        { "yarn", 3 },
        { "aroma", 4 },
        { "mouse", 5 },
        { "brush", 6 },
        { "blanket", 7 },
    };

    public void Start()
    {
      
    }

     public void SetOwner(Cozy cozy)
    {
        cozyOwner = cozy;
    }

    private void OnDestroy()
    {
        if (cozyOwner != null)
        {
            cozyOwner.RemoveCozyBall(gameObject);
        }
    }

    void OnCollisionStay2D(Collision2D collision)
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
                        Destroy(ball.gameObject);
                    }

                    bool completeBonus = true;
                    foreach (bool b in dataObject.PlayerData.GameData.CozyBonus)
                    {
                        if (!b)
                        {
                            completeBonus = false;
                            break;
                        }
                    }

                    float pointChange = 50f * connectedBalls.Count;

                    if (completeBonus) pointChange *= 2f;
            
                    cozyOwner.CreatePopup(Camera.main.WorldToScreenPoint(transform.position), Mathf.FloorToInt(pointChange));
                    dataObject.PlayerData.GameData.UpdateStat("Cozy", Mathf.FloorToInt(pointChange));
                    dataObject.PlayerData.GameData.UpdatePoints(Mathf.FloorToInt(pointChange));
                    
                    

                    if (type != "star")
                    {
                        dataObject.PlayerData.GameData.CozyBonus[labelToIndex[this.type]] = true;
                    }
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
