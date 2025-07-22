using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Cozy : MonoBehaviour
{
    [SerializeField] private DataObject dataObject;
    [SerializeField] private List<GameObject> cozyBallPrefabs = new List<GameObject>();
    [SerializeField] private GameObject previewBallPrefab;
    [SerializeField] private Collider2D playfieldCollider;
    [SerializeField] private Slider capacitySlider;
    [SerializeField] private List<Image> bonusUI;
    [SerializeField] private GameObject bonusText;
    private List<GameObject> cozyBalls = new List<GameObject>();

    private GameObject previewBall;
    private GameObject nextBall;
    private Sprite nextSprite;
    private SpriteRenderer previewSpriteRenderer;

    private Dictionary<string, GameObject> cozyBallMap;

    private int maxCozyBall = 75;

    private readonly string[] cozyBallLabels =
    {
        "warmth", "bed", "tower", "yarn", "aroma", "mouse", "brush", "blanket", "star"
    };
    void Awake()
    {
        cozyBallMap = new Dictionary<string, GameObject>();

        for (int i = 0; i < cozyBallLabels.Length && i < cozyBallPrefabs.Count; i++)
        {
            cozyBallMap[cozyBallLabels[i]] = cozyBallPrefabs[i];
        }

        List<CozySavedBall> csbs = dataObject.PlayerData.GameData.CozySavedBalls;
        foreach (CozySavedBall csb in csbs)
        {
            GameObject prefab = cozyBallMap[csb.Type];
            if (prefab == null) continue;
            GameObject newBall = Instantiate(prefab, transform.localPosition, Quaternion.identity);

            newBall.transform.SetParent(transform);

            Rigidbody2D rb = newBall.GetComponent<Rigidbody2D>();
            if (rb == null) continue;

            newBall.transform.localPosition = new Vector2(csb.XPos, csb.YPos);
            rb.rotation = csb.Rotation;

            cozyBalls.Add(newBall);
            CozyBall cb = newBall.GetComponent<CozyBall>();
            if (cb != null) cb.SetOwner(this);


        }
    }

    void OnDestroy()
    {
        List<CozySavedBall> csbs = new List<CozySavedBall>();
        foreach (GameObject go in cozyBalls)
        {
            Rigidbody2D rb = go.GetComponent<Rigidbody2D>();
            if (rb == null) continue;

            Vector2 pos = go.transform.localPosition;
            float rot = rb.rotation;

            CozyBall cb = go.GetComponent<CozyBall>();
            if (cb == null) continue;
            CozySavedBall csb = new CozySavedBall(pos.x, pos.y, rot, cb.Type);

            if (csb == null) continue;
            csbs.Add(csb);
        }

        dataObject.PlayerData.GameData.CozySavedBalls = csbs;
    }

    void Update()
    {
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (nextBall == null)
        {
            nextBall = cozyBallPrefabs[Random.Range(0, cozyBallPrefabs.Count)];
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

        if (cozyBalls.Count >= maxCozyBall)
        {
            foreach (GameObject go in cozyBalls)
            {
                Destroy(go);
            }
            cozyBalls = new List<GameObject>();

            dataObject.PlayerData.GameData.CozyBonus = new List<bool>{false, false, false, false, false, false, false, false};

        }

        if (capacitySlider)
        {
            capacitySlider.maxValue = maxCozyBall;
            capacitySlider.value = cozyBalls.Count;
        }

        for (int i = 0; i < bonusUI.Count && i < dataObject.PlayerData.GameData.CozyBonus.Count; i++)
        {
            bonusUI[i].color = dataObject.PlayerData.GameData.CozyBonus[i]
            ? new Color(1f, 1f, 1f, 1) : new Color(1f, 1f, 1f, 0.1f);
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

        bonusText.SetActive(completeBonus);
    }

    void CreateBall(Vector2 clampedPosition)
    {
        GameObject newBall = Instantiate(nextBall, clampedPosition, Quaternion.identity);
        newBall.transform.SetParent(transform);
        cozyBalls.Add(newBall);
        CozyBall cb = newBall.GetComponent<CozyBall>();
        if (cb != null) cb.SetOwner(this);

        nextBall = cozyBallPrefabs[Random.Range(0, cozyBallPrefabs.Count)];
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
    
    public void RemoveCozyBall(GameObject ball)
    {
        if (cozyBalls.Contains(ball))
        {
            cozyBalls.Remove(ball);
        }
    }
}
