using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class Love : MonoBehaviour
{
    [SerializeField] private DataObject dataObject;
    [SerializeField] private Transform heartTransform;
    [SerializeField] private float moveDuration = 0.5f;
    [SerializeField] private RectTransform canvasRectTransform;
    [SerializeField] private Animator animator;
    [SerializeField] private Canvas canvas;
    [SerializeField] private List<AudioClip> purrs;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Image image;
    [SerializeField] private Sprite loveHeartSprite, goldHeartSprite;
    private int coinValue;

    private float canvasXPadding;
    private float canvasYPadding;
    private bool isMoving = false;
    private Vector3 targetPosition;
    private float moveTimer = 0f;
    private UI ui;

    private int heartValue;

    void Start()
    {
        GameObject go = GameObject.Find("UI");
        if (go) ui = go.GetComponent<UI>();
    }
    void Awake()
    {
        Vector2 randomScreenPoint = new Vector2(
           Random.Range(canvasXPadding, Screen.width - canvasXPadding),
           Random.Range(canvasYPadding, Screen.height - canvasYPadding)
       );

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, randomScreenPoint, null, out localPoint);

        heartTransform.localPosition = new Vector3(localPoint.x, localPoint.y, 0f);
    }

    void Update()
    {
        UpdateUpgradeValues();

        if (isMoving)
        {
            moveTimer += Time.deltaTime;
            float t = moveTimer / moveDuration;
            heartTransform.localPosition = Vector3.Lerp(heartTransform.localPosition, targetPosition, t);

            if (t >= 1f)
            {
                isMoving = false;
            }
        }
    }


    public void HeartPress()
    {
        dataObject.PlayerData.GameData.UpdateStat("Love", heartValue);
        dataObject.PlayerData.GameData.UpdatePoints(heartValue);
        dataObject.PlayerData.GameData.Coins += coinValue;

        if (animator) animator.SetTrigger("purr");

        if (ui) ui.SpawnPopup(heartTransform.position, "Love", heartValue, canvas.transform);

        if (audioSource)
        {
            audioSource.clip = purrs[Random.Range(0, purrs.Count)];
            audioSource.pitch = Random.Range(1.00f - 0.10f, 1.00f + 0.10f);
            audioSource.Play();
        }

        StartHeartMove();
    }

    private void StartHeartMove()
    {
        Vector2 randomScreenPoint = new Vector2(
            Random.Range(canvasXPadding, Screen.width - canvasXPadding),
            Random.Range(canvasYPadding, Screen.height - canvasYPadding)
        );

        Vector2 localPoint = randomScreenPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, randomScreenPoint, null, out localPoint);

        if (heartTransform != null)
        {
            targetPosition = new Vector3(localPoint.x, localPoint.y, 0f);
            moveTimer = 0f;
            isMoving = true;
        }
    }

    void UpdateUpgradeValues()
    {
        List<bool> upgrades = dataObject.PlayerData.GameData.Upgrades;
        Stat love = dataObject.PlayerData.GameData.Stats[0];
        if (upgrades[2])
        {
            heartValue = 100;
            heartTransform.localScale = Vector3.one * 2f;
        }
        else if (upgrades[1])
        {
            heartValue = 75;
            heartTransform.localScale = Vector3.one * 1.5f;
        }
        else if (upgrades[0])
        {
            heartValue = 50;
            heartTransform.localScale = Vector3.one * 1.25f;
        }
        else
        {
            heartValue = 25;
            heartTransform.localScale = Vector3.one;
        }

        if (upgrades[3])
        {
            if (image) image.sprite = goldHeartSprite;
            coinValue = 50;
        }
        else
        {
            if (image) image.sprite = loveHeartSprite;
            coinValue = 0;
        }

        if (upgrades[4])
        {
            canvasXPadding = 300f;
            canvasYPadding = 400f;
        }
        else
        {
            canvasXPadding = 64f;
            canvasYPadding = 400f;
        }
        
    }
}
