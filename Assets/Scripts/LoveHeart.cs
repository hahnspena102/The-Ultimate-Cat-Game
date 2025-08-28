using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class LoveHeart : MonoBehaviour
{
    [Header("Basics")]
    [SerializeField] private DataObject dataObject;
    [SerializeField] private Transform heartTransform;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Image image;
    [ReadOnly, SerializeField] private Love love;

    private UI ui;

    [Header("Values")]
    [SerializeField] private Values values;
    [SerializeField] private float moveDuration = 0.5f;
    private float moveTimer = 0f;
    private bool isMoving = false;
    private Vector3 targetPosition;

    [Header("Audio Clips & Sprites")]
    [SerializeField] private List<AudioClip> purrs;
    [SerializeField] private Sprite loveHeartSprite;
    [SerializeField] private Sprite goldHeartSprite;


    void Awake()
    {
        GameObject go = GameObject.Find("UI");
        if (go) ui = go.GetComponent<UI>();

        love = GameObject.FindFirstObjectByType<Love>();

        Vector2 randomScreenPoint = NewRandomScreenPoint();

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(love.CanvasRectTransform, randomScreenPoint, null, out localPoint);

        heartTransform.localPosition = new Vector3(localPoint.x, localPoint.y, 0f);
    }

    Vector2 NewRandomScreenPoint()
    {
        return new Vector2(
            Random.Range(values.LoveCanvasXPadding, Screen.width - values.LoveCanvasXPadding),
            Random.Range(values.LoveCanvasYPadding, Screen.height - values.LoveCanvasYPadding)
        );
    }

    void Update()
    {
        UpdateUpgradeVisuals();
        heartTransform.localScale = Vector3.one * values.LoveHeartSizeMultiplier;

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
        int pointChange = values.LoveHeartValue;
        dataObject.PlayerData.GameData.UpdateStat("Love", pointChange);
        dataObject.PlayerData.GameData.UpdatePoints(pointChange);

        if (values.LoveCoinValue > 0)
        {
            int coinChange = values.LoveCoinValue;
            dataObject.PlayerData.GameData.Coins += coinChange;
            if (ui) ui.SpawnCoinPopup(coinChange, love.Canvas.transform);
        }

        if (love.Animator) love.Animator.SetTrigger("purr");

        if (ui) ui.SpawnPopup(heartTransform.position, "Love", pointChange, love.Canvas.transform);

        if (audioSource) Util.PlaySFX(audioSource, purrs[Random.Range(0, purrs.Count)]);

        StartHeartMove();
    }

    private void StartHeartMove()
    {
        Vector2 randomScreenPoint = NewRandomScreenPoint();

        Vector2 localPoint = randomScreenPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(love.CanvasRectTransform, randomScreenPoint, null, out localPoint);

        if (heartTransform != null)
        {
            targetPosition = new Vector3(localPoint.x, localPoint.y, 0f);
            moveTimer = 0f;
            isMoving = true;
        }
    }

    void UpdateUpgradeVisuals()
    {
        List<bool> upgrades = dataObject.PlayerData.GameData.Upgrades;

        if (upgrades[3])
        {
            if (image) image.sprite = goldHeartSprite;
        }
        else
        {
            if (image) image.sprite = loveHeartSprite;
        }   
    }
}
