using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
public class PurchasePanel : MonoBehaviour
{
    [SerializeField]private DataObject dataObject;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TMPro.TextMeshProUGUI nameTextBox, descTextBox, costTextBox, prereqTextBox;
    [SerializeField] private Image iconImage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (dataObject.SelectedUpgrade != null)
        {
            canvasGroup.alpha = 1;
            nameTextBox.text = dataObject.SelectedUpgrade.UpgradeName;
            descTextBox.text = dataObject.SelectedUpgrade.Description;
            costTextBox.text = $"{dataObject.SelectedUpgrade.Cost}";
            iconImage.sprite = dataObject.SelectedUpgrade.Sprite;

            if (dataObject.SelectedUpgrade.Prerequisites.Count > 0)
            {
                List<string> pNames = new List<string>();
                foreach (Upgrade u in dataObject.SelectedUpgrade.Prerequisites)
                {
                    if (u == null) continue;
                    pNames.Add(u.UpgradeName);
                }
                prereqTextBox.text = "Prerequisites: " + String.Join(", ", pNames);
            }
            else
            {
                prereqTextBox.text = "";
            }
        }
        else
        {
            canvasGroup.alpha = 0;
        }
    }
}
