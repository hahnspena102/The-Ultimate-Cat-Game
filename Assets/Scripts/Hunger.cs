using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Hunger : MonoBehaviour

{
    [SerializeField] private DataObject dataObject;
    [SerializeField] private HungerTable hungerTable;
    [SerializeField] private TMPro.TextMeshProUGUI currentComboText;
    [SerializeField] private FoodCombo currentFoodCombo;
    [SerializeField] private Transform receipt;
    [SerializeField] private GameObject receiptBlock;
    [SerializeField] private Slider appetiteSlider;
    [SerializeField] private Button feedButton;
    [SerializeField] private List<AudioClip> noms;
    [SerializeField] private AudioSource nomSource;
    private int maxReceiptBlocks = 10;
    private UI ui;

    void Start()
    {
        GameObject go = GameObject.Find("UI");
        if (go) ui = go.GetComponent<UI>();

        StartCoroutine(GeneratorCoroutine());
    }

    void UpdateComboText()
    {
        int pointChange = currentFoodCombo.Calculate();

        currentComboText.text = currentFoodCombo.ToString();

        Color targetColor = new Color(0f, 0f, 0f, 0f);

        if (dataObject.PlayerData.GameData.Upgrades[9]) // Glutton Gaze 1
        {
            if (currentFoodCombo.Food.Effect == "Miscellaneous")
            {
                if (dataObject.PlayerData.GameData.Upgrades[10]) targetColor = new Color(0.380f, 0.961f, 0.980f, 1.000f);
            }
            else if (pointChange <= -500)
            {
                targetColor = new Color(0.612f, 0.224f, 0.973f, 1.000f);
            }
            else if (dataObject.PlayerData.GameData.Upgrades[10]) // Glutton Gaze 2
            {
                if (dataObject.PlayerData.GameData.Upgrades[11]) // Glutton Gaze 3
                {
                    if (pointChange > 0)
                    {
                        float t = Mathf.Clamp01(pointChange / 500f);
                        targetColor = Color.Lerp(new Color(0.780f, 0.769f, 0.078f, 1.000f), new Color(0.173f, 0.694f, 0.173f, 1.000f), t);
                    }
                    else
                    {
                        float t = Mathf.Clamp01(-pointChange / 500f);
                        targetColor = Color.Lerp(new Color(0.827f, 0.565f, 0.075f, 1.000f), new Color(1.000f, 0.161f, 0.161f, 1.000f), t);
                    }
                }
                else
                {
                    int bigValueThreshold = 150;
                    if (pointChange >= bigValueThreshold)
                    {
                        targetColor = new Color(0.247f, 0.773f, 0.247f, 1.000f);
                    }
                    else if (pointChange <= -bigValueThreshold)
                    {
                        targetColor = new Color(1.000f, 0.161f, 0.161f, 1.000f);
                    }

                }
                
            }
        }
        

        currentComboText.fontMaterial.SetColor("_OutlineColor", targetColor);
        currentComboText.fontMaterial.SetFloat("_OutlineWidth", 0.12f);
    }
    void Update()
    {
        if (currentComboText) {
            UpdateComboText();
        }

        if (appetiteSlider && feedButton)
        {
            appetiteSlider.maxValue = dataObject.PlayerData.GameData.MaxAppetite;
            appetiteSlider.value = dataObject.PlayerData.GameData.Appetite;

            float percent = (float)(dataObject.PlayerData.GameData.Appetite / (float)dataObject.PlayerData.GameData.MaxAppetite);

            if (percent < 0.1)
            {
                feedButton.interactable = false;

                ColorBlock colors = appetiteSlider.colors;
                colors.disabledColor = new Color(0.659f, 0.220f, 0.086f, 1.000f);
                appetiteSlider.colors = colors;

            }
            else if (percent == 1)
            {
                feedButton.interactable = true;

                ColorBlock colors = appetiteSlider.colors;
                colors.disabledColor = new Color(1f, 0.72549020f, 0f, 1f);

                appetiteSlider.colors = colors;

            }
                
            
        }

        
    }

    IEnumerator GeneratorCoroutine()
    {
        currentFoodCombo = GenerateCombo();
        yield return new WaitForSeconds(1f);
        StartCoroutine(GeneratorCoroutine());
    }

    FoodCombo GenerateCombo()
    {
        List<Food> foods = hungerTable.Foods;
        Food randomFood = foods[Random.Range(0, foods.Count)];

        List<Adjective> adjectives = FilterAdjectives(hungerTable.Adjectives, randomFood.Type);


        Adjective randomAdjective = adjectives[Random.Range(0, adjectives.Count)];

        return new FoodCombo(randomFood, randomAdjective);
    }

    List<Adjective> FilterAdjectives(List<Adjective> adjectives, string type)
    {
        List<Adjective> filtered = new List<Adjective>();

        foreach (Adjective adj in adjectives)
        {
            if (type != "None")
            {
                if (adj.Type == type || adj.Type == "Generic")
                {
                    filtered.Add(adj);
                }
            }
            else
            {
                if (adj.Type == type)
                {
                    filtered.Add(adj);
                }
            }
        }

        return filtered;
    }

    public void Feed()
    {
        int pointChange = currentFoodCombo.Calculate();
        if (dataObject)
        {
            dataObject.PlayerData.GameData.UpdateStat("Hunger", pointChange);
            dataObject.PlayerData.GameData.UpdatePoints(pointChange);
        }
        ;
        

        if (receiptBlock && receipt)
        {
            GameObject newBlock = Instantiate(receiptBlock, transform.position, Quaternion.identity);
            newBlock.transform.SetParent(receipt);
            newBlock.transform.localScale = Vector3.one;

            ReceiptBlock rb = newBlock.GetComponent<ReceiptBlock>();
            if (rb) {
                if (currentFoodCombo.Food.Effect == "Miscellaneous")
                {
                    rb.Type = "Miscellaneous";
                }
                else if (pointChange > 0)
                {
                    rb.Type = "Good";
                }
                else if (pointChange < 0)
                {
                    rb.Type = "Bad";
                }
            }
            TMPro.TextMeshProUGUI tm = newBlock.transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>();
            if (tm) tm.text = $"{currentFoodCombo.ToString()}";

            tm = newBlock.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>();
            if (tm) tm.text = $"{pointChange}";

            if (receipt.childCount > maxReceiptBlocks)
            {
                Transform lastChild = receipt.GetChild(0);
                Destroy(lastChild.gameObject);
            }
        }

        dataObject.PlayerData.GameData.Appetite = Mathf.Max(0, dataObject.PlayerData.GameData.Appetite -= 25);

        // SFX
        if (nomSource)
        {
            nomSource.clip = noms[Random.Range(0, noms.Count)];
            nomSource.pitch = Random.Range(1.00f - 0.10f, 1.00f + 0.10f);
            nomSource.Play();
        }

        float pw = 200f;
        float ph = 50f;
        Vector2 spawnPosition = new Vector2(Random.Range(currentComboText.transform.position.x - pw, currentComboText.transform.position.x + pw),
                                            Random.Range(currentComboText.transform.position.y - ph, currentComboText.transform.position.y + ph));
        if (ui) ui.SpawnPopup(spawnPosition, "Hunger", pointChange, transform);
    }
}
