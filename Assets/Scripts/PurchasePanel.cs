using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
public class PurchasePanel : MonoBehaviour
{
    [SerializeField] private DataObject dataObject;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TMPro.TextMeshProUGUI nameTextBox, descTextBox, costTextBox, prereqTextBox, purchaseText;
    [SerializeField] private Image iconImage;
    [SerializeField] private Button purchaseButton;

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

            bool isPurchased = dataObject.PlayerData.GameData.Upgrades[dataObject.SelectedUpgrade.Id];
            if (isPurchased)
            {
                purchaseButton.interactable = false;
                purchaseText.text = "Purchased";
                purchaseButton.image.color = new Color(0.592f, 0.596f, 0.627f, 1.000f);
                costTextBox.color = Color.white;
            }
            else if (!IsPurchasable())
            {
                purchaseButton.interactable = false;
                purchaseText.text = "Buy";
                purchaseButton.image.color = new Color(0.961f, 0.365f, 0.365f, 1.000f);
                costTextBox.color = new Color(0.961f, 0.365f, 0.365f, 1.000f);
            }
            else
            {
                purchaseButton.interactable = true;
                purchaseText.text = "Buy";
                purchaseButton.image.color = Color.white;
                costTextBox.color = Color.white;
            }
        }
        else
        {
            canvasGroup.alpha = 0;
        }
    }

    public void Purchase()
    {
        if (dataObject.PlayerData.GameData.Coins >= dataObject.SelectedUpgrade.Cost)
        {
            List<bool> upgrades = dataObject.PlayerData.GameData.Upgrades;
            int selectedId = dataObject.SelectedUpgrade.Id;

            if (selectedId < 0 || selectedId >= upgrades.Count) return;

            upgrades[selectedId] = true;

            dataObject.PlayerData.GameData.Coins -= dataObject.SelectedUpgrade.Cost;
        }
    }

    public bool IsPurchasable()
    {
        foreach (Upgrade prereq in dataObject.SelectedUpgrade.Prerequisites)
        {
            if (!dataObject.PlayerData.GameData.Upgrades[prereq.Id]) return false;
        }
        return dataObject.PlayerData.GameData.Coins >= dataObject.SelectedUpgrade.Cost;
    }
}
