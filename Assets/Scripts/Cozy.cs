using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Cozy : MonoBehaviour
{
    [SerializeField]private List<GameObject> cozyBalls = new List<GameObject>();
    [SerializeField]private GameObject previewBallPrefab;
    [SerializeField]private Collider2D playfieldCollider;

    private GameObject previewBall;
    private GameObject nextBall;
    private Sprite nextSprite;

    private SpriteRenderer previewSpriteRenderer;


    void Update()
    {
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (nextBall == null)
        {
            nextBall = cozyBalls[Random.Range(0, cozyBalls.Count)];
        }

        if (previewBall == null)
        {
            previewBall = Instantiate(previewBallPrefab);
            previewBall.transform.SetParent(transform);
            previewSpriteRenderer = previewBall.GetComponent<SpriteRenderer>();
        }

        Vector2 clampedPosition = ClampToPlayfield(mouseWorldPos);
        previewBall.transform.position = clampedPosition;

        if (previewSpriteRenderer) previewSpriteRenderer.sprite = nextSprite;

        if (Input.GetMouseButtonDown(0))
        {
            CreateBall(clampedPosition);
        }
    }

    void CreateBall(Vector2 clampedPosition)
    {
        GameObject newBall = Instantiate(nextBall, clampedPosition, Quaternion.identity);
        newBall.transform.SetParent(transform);


        nextBall = cozyBalls[Random.Range(0, cozyBalls.Count)];
        SpriteRenderer sr = nextBall.GetComponent<SpriteRenderer>();
        if (sr) nextSprite = sr.sprite;
    }

    
    Vector2 ClampToPlayfield(Vector2 position)
    {
        Bounds bounds = playfieldCollider.bounds;

        float clampedX = Mathf.Clamp(position.x, bounds.min.x, bounds.max.x);
        float clampedY = Mathf.Clamp(position.y, bounds.min.y, bounds.max.y);

        return new Vector2(clampedX, clampedY);
    }
}
