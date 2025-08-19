using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class ValueUpdateDisplay : MonoBehaviour
{
    [SerializeField] private DataObject dataObject;
    [SerializeField] private TextMeshProUGUI textMesh;
    [SerializeField] private ValueType valueType;
    [SerializeField] private int previousValue;
    [SerializeField] private int totalChange;
    [SerializeField] private int changeInValue;
    [SerializeField] private float elapsedTime;
    private float maxTransparency = 0.3f;
    private float displayDurationShort = 0.5f;
    private float displayDurationLong = 3f;
    private int changeThreshold;
    private bool isFading = false;

    public enum ValueType
    {
        Points,
        Coins
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        previousValue = GetValue();

        switch (valueType)
        {
            case ValueType.Points:
                changeThreshold = 15;
                break;
            case ValueType.Coins:
                changeThreshold = 3;
                break;
            default:
                break;

        }
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime = Mathf.Max(0, elapsedTime - Time.deltaTime);

        if (textMesh == null) return;

        changeInValue = GetValue() - previousValue;
        previousValue = GetValue();
        totalChange += changeInValue;


        if (changeInValue > changeThreshold)
        {
            elapsedTime = displayDurationLong;
            textMesh.text = $"+{totalChange}";
        }
        else if (changeInValue > 0)
        {
            elapsedTime = Mathf.Max(elapsedTime, displayDurationShort);
            textMesh.text = $"+{totalChange}";
        }

        if (elapsedTime <= 0f)
        {
            //textMesh.color = new Color(1f, 1f, 1f, 0f);
            if (!isFading) FadeOut();
            totalChange = 0;
        }
        else
        {
            textMesh.color = new Color(1f, 1f, 1f, maxTransparency);
            isFading = false;
        }

    }

    private int GetValue()
    {
        switch (valueType)
        {
            case ValueType.Points:
                return dataObject.PlayerData.GameData.Points;
            case ValueType.Coins:
                return dataObject.PlayerData.GameData.Coins;
            default:
                return 0;

        }
    }


    private void FadeOut()
    {
        StartCoroutine(FadeOutCoroutine());
    }

    private IEnumerator FadeOutCoroutine()
    {
        isFading = true;
        float popupElapsed = 0f;
        float fadeDuration = 1f;


        while (popupElapsed < fadeDuration)
        {
            float alpha = Mathf.Lerp(maxTransparency, 0f, popupElapsed / fadeDuration);
            textMesh.color = new Color(1f, 1f, 1f, alpha);

            popupElapsed += Time.deltaTime;
            yield return null; 
        }


        isFading = false;
    }
    


}
