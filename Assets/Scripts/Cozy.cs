using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
public class Cozy : MonoBehaviour
{
    [Header("Basics")]
    [SerializeField] private DataObject dataObject;
    [SerializeField] private List<GameObject> cozyBallPrefabs = new List<GameObject>();
    [SerializeField] private Collider2D playfieldCollider;
    [SerializeField] private Slider capacitySlider;
    [SerializeField] private List<Image> bonusUI;
    [SerializeField] private GameObject bonusText;
    [SerializeField] private SpriteRenderer previewSpriteRenderer, nextSpriteRenderer;
    [SerializeField] private Canvas canvas;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private TextMeshProUGUI multiplierTextMesh;
    [SerializeField] private TextMeshProUGUI bonusTimerTextMesh;
    [SerializeField] private GameObject nextUpBox, nextUpBinding;
    private GameObject currentBallPrefab, nextBallPrefab;
    private Sprite nextSprite, currentSprite;

    private UI ui;

    [Header("Values")]
    [SerializeField] private Values values;
    private List<GameObject> cozyBalls = new List<GameObject>();
    private Dictionary<string, GameObject> cozyBallMap;
    private int overflowPenalty = -500;
    private float elapsedTime = 0f;
    private float bonusDuration = 30f;

    private readonly string[] cozyBallLabels =
    {
        "warmth", "bed", "tower", "yarn", "aroma", "mouse", "brush", "blanket", "star"
    };

    [Header("Audio Clips & Sprites")]

    [SerializeField] private AudioClip connectionSFX;
    [SerializeField] private AudioClip bonusSFX, resetSFX;

    public global::System.Single BonusDuration { get => bonusDuration; set => bonusDuration = value; }

    void Awake()
    {
        GameObject go = GameObject.Find("UI");
        if (go) ui = go.GetComponent<UI>();

        if (previewSpriteRenderer != null)
        {
            nextBallPrefab = cozyBallPrefabs[Random.Range(0, cozyBallPrefabs.Count)];
            currentBallPrefab = cozyBallPrefabs[Random.Range(0, cozyBallPrefabs.Count)];

            SpriteRenderer csr = currentBallPrefab.GetComponent<SpriteRenderer>();
            if (csr) currentSprite = csr.sprite;

            SpriteRenderer nsr = nextBallPrefab.GetComponent<SpriteRenderer>();
            if (nsr) nextSprite = nsr.sprite;
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
        if (dataObject.PlayerData.GameData.CozyBonusTimer <= 0) values.CozyMultiplier = 1f;

        if (multiplierTextMesh) multiplierTextMesh.text = $"x{values.CozyMultiplier} Bonus";
        if (bonusTimerTextMesh) bonusTimerTextMesh.text = dataObject.PlayerData.GameData.CozyBonusTimer > 0 ? $"{Mathf.RoundToInt(dataObject.PlayerData.GameData.CozyBonusTimer)}" : "";

        dataObject.PlayerData.GameData.CozyBonusTimer = Mathf.Max(0, dataObject.PlayerData.GameData.CozyBonusTimer - Time.deltaTime);



        elapsedTime += Time.deltaTime;
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (nextBallPrefab == null)
        {
            nextBallPrefab = cozyBallPrefabs[Random.Range(0, cozyBallPrefabs.Count)];
        }

        Vector2 clampedPosition = ClampToPlayfield(mouseWorldPos);
        previewSpriteRenderer.transform.position = clampedPosition;

        if (previewSpriteRenderer)
        {
            previewSpriteRenderer.sprite = currentSprite;
            if (elapsedTime >= values.CozyCooldown)
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

        nextUpBox.SetActive(dataObject.PlayerData.GameData.Upgrades[49]);
        nextUpBinding.SetActive(dataObject.PlayerData.GameData.Upgrades[49]);

        if (Input.GetMouseButtonDown(1) && dataObject.PlayerData.GameData.Upgrades[49])
        {
            SwapBall();
        }

        if (nextSpriteRenderer)
        {
            nextSpriteRenderer.sprite = nextSprite;
        }

        if (cozyBalls.Count >= values.MaxCozyBall)
        {
            ResetBasket();
        }

        if (capacitySlider)
        {
            capacitySlider.maxValue = values.MaxCozyBall;
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
        GameObject newBall = Instantiate(currentBallPrefab, clampedPosition, Quaternion.identity);
        newBall.transform.SetParent(transform);
        cozyBalls.Add(newBall);
        CozyBall cb = newBall.GetComponent<CozyBall>();
        if (cb != null) cb.SetOwner(this);

        currentBallPrefab = nextBallPrefab;


        nextBallPrefab = cozyBallPrefabs[Random.Range(0, cozyBallPrefabs.Count)];

        SpriteRenderer csr = currentBallPrefab.GetComponent<SpriteRenderer>();
        if (csr) currentSprite = csr.sprite;

        SpriteRenderer nsr = nextBallPrefab.GetComponent<SpriteRenderer>();
        if (nsr) nextSprite = nsr.sprite;

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

    public void SwapBall()
    {
        GameObject temp = nextBallPrefab;
        Sprite tempSprite = nextSprite;
        nextBallPrefab = currentBallPrefab;
        nextSprite = currentSprite;
        currentBallPrefab = temp;
        currentSprite = tempSprite;

    }
}
