using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
public class Cozy : MonoBehaviour
{
    [SerializeField] private DataObject dataObject;
    [SerializeField] private List<GameObject> cozyBallPrefabs = new List<GameObject>();
    [SerializeField] private Collider2D playfieldCollider;
    [SerializeField] private Slider capacitySlider;
    [SerializeField] private List<Image> bonusUI;
    [SerializeField] private GameObject bonusText;
    private List<GameObject> cozyBalls = new List<GameObject>();

    [SerializeField] private GameObject previewBall;
    [SerializeField] private Canvas canvas;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip connectionSFX, bonusSFX, resetSFX;
    [SerializeField] private TextMeshProUGUI multiplierTextMesh;
    [SerializeField] private TextMeshProUGUI bonusTimerTextMesh;
    private GameObject nextBall;
    private Sprite nextSprite;
    private SpriteRenderer previewSpriteRenderer;

    private Dictionary<string, GameObject> cozyBallMap;

    private int maxCozyBall = 30;
    private int overflowPenalty = -500;

    private UI ui;
    private float elapsedTime = 0f;
    private float cooldown = 1f;

    private float cozyMultiplier;
    private int cozyValue;
    private float bonusDuration = 30f;

    private readonly string[] cozyBallLabels =
    {
        "warmth", "bed", "tower", "yarn", "aroma", "mouse", "brush", "blanket", "star"
    };

    public global::System.Single CozyMultiplier { get => cozyMultiplier; set => cozyMultiplier = value; }
    public global::System.Int32 CozyValue { get => cozyValue; set => cozyValue = value; }
    public global::System.Single BonusDuration { get => bonusDuration; set => bonusDuration = value; }

    void Awake()
    {
        GameObject go = GameObject.Find("UI");
        if (go) ui = go.GetComponent<UI>();

        if (previewBall != null)
        {
            previewSpriteRenderer = previewBall.GetComponent<SpriteRenderer>();
            nextBall = cozyBallPrefabs[Random.Range(0, cozyBallPrefabs.Count)];
            SpriteRenderer sr = nextBall.GetComponent<SpriteRenderer>();
            if (sr) nextSprite = sr.sprite;
        }

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
        UpdateUpgradeValues();

        // BONUS
        bool completeBonus = true;
        foreach (bool b in dataObject.PlayerData.GameData.CozyBonus)
        {
            if (!b)
            {
                completeBonus = false;
                break;
            }
        }
        if (dataObject.PlayerData.GameData.CozyBonusTimer <= 0) cozyMultiplier = 1f;

        if (multiplierTextMesh) multiplierTextMesh.text = $"x{cozyMultiplier} Bonus";
        if (bonusTimerTextMesh) bonusTimerTextMesh.text = dataObject.PlayerData.GameData.CozyBonusTimer > 0 ? $"{Mathf.RoundToInt(dataObject.PlayerData.GameData.CozyBonusTimer)}" : "";

        dataObject.PlayerData.GameData.CozyBonusTimer = Mathf.Max(0, dataObject.PlayerData.GameData.CozyBonusTimer - Time.deltaTime);
        


        elapsedTime += Time.deltaTime;
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (nextBall == null)
        {
            nextBall = cozyBallPrefabs[Random.Range(0, cozyBallPrefabs.Count)];
        }

        Vector2 clampedPosition = ClampToPlayfield(mouseWorldPos);
        previewBall.transform.position = clampedPosition;

        if (previewSpriteRenderer)
        {
            previewSpriteRenderer.sprite = nextSprite;
            if (elapsedTime >= cooldown)
            {
                previewSpriteRenderer.color = new Color(1f, 1f, 1f, 0.3f);
                if (Input.GetMouseButtonDown(0))
                {
                    elapsedTime = 0;
                    CreateBall(clampedPosition);
                }



            }
            else
            {
                previewSpriteRenderer.color = new Color(0.478f, 0.478f, 0.478f, 0.000f);
            }
        }





        if (cozyBalls.Count >= maxCozyBall)
        {
            ResetBasket();
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

        if (!bonusText.activeSelf && completeBonus)
        {
            dataObject.PlayerData.GameData.CozyBonus = new List<bool> { false, false, false, false, false, false, false, false };
            dataObject.PlayerData.GameData.CozyBonusTimer = bonusDuration;
            Util.PlaySFX(audioSource, bonusSFX, 0f);
        }

        bonusText.SetActive(dataObject.PlayerData.GameData.CozyBonusTimer > 0f);
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

        AudioSource src = newBall.GetComponent<AudioSource>();
        Util.PlaySFX(src, null, 0.2f);
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

    public void CreatePopup(Vector3 spawnPosition, int pointChange)
    {
        if (ui) ui.SpawnPopup(spawnPosition, "Cozy", pointChange, canvas.transform);
    }

    void ResetBasket()
    {

        foreach (GameObject go in cozyBalls)
        {
            Destroy(go);
        }
        cozyBalls = new List<GameObject>();

        dataObject.PlayerData.GameData.CozyBonus = new List<bool> { false, false, false, false, false, false, false, false, false };

        dataObject.PlayerData.GameData.UpdateStat("Cozy", overflowPenalty);
        float pw = 100f;
        float ph = 50f;
        Vector2 spawnPosition = new Vector2(Random.Range(canvas.transform.position.x - pw, canvas.transform.position.x + pw),
                                            Random.Range(canvas.transform.position.y - ph, canvas.transform.position.y + ph));

        if (ui) ui.SpawnPopup(spawnPosition, "Cozy", overflowPenalty, canvas.transform);

        Util.PlaySFX(audioSource, resetSFX, 0f);
    }

    public void ConnectionSFX()
    {
        Util.PlaySFX(audioSource, connectionSFX, 0.2f);
    }

    void UpdateUpgradeValues()
    {
        List<bool> upgrades = dataObject.PlayerData.GameData.Upgrades;

        if (upgrades[47])
        {
            cooldown = 0.25f;
            maxCozyBall = 60;
        }
        else if (upgrades[46])
        {
            cooldown = 0.5f;
            maxCozyBall = 50;
        }
        else if (upgrades[45])
        {
            cooldown = 0.75f;
            maxCozyBall = 40;
        }
        else
        {
            cooldown = 1f;
            maxCozyBall = 30;
        }
        
        if (upgrades[51])
        {
            cozyValue = 200;
            cozyMultiplier = 4f;
        }
        else if (upgrades[50])
        {
            cozyValue = 100;
            cozyMultiplier = 3f;
        }
        else 
        {
            cozyValue = 50;
            cozyMultiplier = 2f;
        }
    }
}
