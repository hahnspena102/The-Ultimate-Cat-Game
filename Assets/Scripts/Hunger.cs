using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Hunger : MonoBehaviour

{
    [SerializeField] private DataObject dataObject;
    [SerializeField] private HungerTable hungerTable;
    [SerializeField] private TMPro.TextMeshProUGUI currentComboText;
    [SerializeField] private FoodCombo currentFoodCombo, nextFoodCombo;
    [SerializeField] private Transform receipt;
    [SerializeField] private GameObject receiptBlock;
    [SerializeField] private Slider appetiteSlider;
    [SerializeField] private Button feedButton;
    [SerializeField] private List<AudioClip> noms;
    [SerializeField] private AudioSource nomSource;
    [SerializeField] private Image nextFoodComboFill;
    [SerializeField] private GameObject futureVision;
    private int maxReceiptBlocks = 10;
    private UI ui;
    private float elapsedTime;

    void Awake()
    {
        GameObject go = GameObject.Find("UI");
        if (go) ui = go.GetComponent<UI>();

        nextFoodCombo = GenerateCombo();
        GenerateNewCombo();
    }

    void UpdateComboVisuals()
    {

        int gluttonGazeLevel = 0;

        if (currentComboText)
        {
                currentComboText.text = currentFoodCombo.ToString();

            Color targetColor = Color.white;
            if (dataObject.PlayerData.GameData.Upgrades[11])
            {
                gluttonGazeLevel = 3;
            } else if (dataObject.PlayerData.GameData.Upgrades[10])
            {
                gluttonGazeLevel = 2;
            }
            else if (dataObject.PlayerData.GameData.Upgrades[9])
            {
                gluttonGazeLevel = 1;
            }


            targetColor = currentFoodCombo.GetColor(gluttonGazeLevel);
            currentComboText.fontMaterial.SetColor("_OutlineColor", targetColor);
            currentComboText.fontMaterial.SetFloat("_OutlineWidth", 0.12f);
        }
        
        if (nextFoodComboFill)
        {
            nextFoodComboFill.color = nextFoodCombo.GetColor(gluttonGazeLevel);
        }
    }

    void GenerateNewCombo()
    {
        currentFoodCombo = nextFoodCombo;
        nextFoodCombo = GenerateCombo();
        elapsedTime = 0f;

    }
    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime > 1f)
        {
            GenerateNewCombo();
        }
        
        UpdateComboVisuals();

        futureVision.SetActive(dataObject.PlayerData.GameData.Upgrades[15]);
        


        if (appetiteSlider && feedButton)
        {
            appetiteSlider.maxValue = dataObject.PlayerData.GameData.Appetite.MaxValue;
            appetiteSlider.value = dataObject.PlayerData.GameData.Appetite.Value;

            float percent = dataObject.PlayerData.GameData.Appetite.Percent();

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

        if (currentFoodCombo.Food.Effect == "Miscellaneous")
        {
            string foodName = currentFoodCombo.Food.FoodName;
            if (foodName == "Catnip")
            {
                if (dataObject.PlayerData.GameData.UpdateStat("Clean", -100) != 0)
                {
                    CreatePopup("Clean", -100);
                }

                if (dataObject.PlayerData.GameData.UpdateStat("Love", 100) != 0)
                {
                    CreatePopup("Love", 100);
                }
            }
            else if (foodName == "Dog Food")
            {
                if (dataObject.PlayerData.GameData.UpdateStat("Love", -100) != 0)
                {
                    CreatePopup("Love", -100);
                }
            }
            else if (foodName == "Cucumber")
            {
                if (dataObject.PlayerData.GameData.UpdateStat("Soul", -200) != 0)
                {
                    CreatePopup("Soul", -200);
                }
                if (dataObject.PlayerData.GameData.UpdateStat("Love", -50) != 0)
                {
                    CreatePopup("Love", -50);
                }
                if (dataObject.PlayerData.GameData.UpdateStat("Energy", 100) != 0)
                {
                    CreatePopup("Energy", 100);
                }
                if (dataObject.PlayerData.GameData.UpdateStat("Cozy", -100) != 0)
                {
                    CreatePopup("Cozy", -100);
                }
            }
            else if (foodName == "Mysterious Essence")
            {
                List<string> types = new List<string> { "Love", "Thirst", "Energy", "Clean", "Cozy", "Health", "Lifeforce", "Soul" };
                foreach (string t in types)
                {
                    if (dataObject.PlayerData.GameData.UpdateStat(t, 55) != 0)
                    {
                        CreatePopup(t, 55);
                    }
                }


            }
        }


        if (receiptBlock && receipt)
        {
            GameObject newBlock = Instantiate(receiptBlock, transform.position, Quaternion.identity);
            newBlock.transform.SetParent(receipt);
            newBlock.transform.localScale = Vector3.one;

            ReceiptBlock rb = newBlock.GetComponent<ReceiptBlock>();
            if (rb)
            {
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

        dataObject.PlayerData.GameData.Appetite.Update(-25);

        // SFX
        if (nomSource)
        {
            nomSource.clip = noms[Random.Range(0, noms.Count)];
            nomSource.pitch = Random.Range(1.00f - 0.10f, 1.00f + 0.10f);
            nomSource.Play();
        }

        CreatePopup("Hunger", pointChange);
    }

    public void NextFood()
    {
        dataObject.PlayerData.GameData.Appetite.Update(-100);
        GenerateNewCombo();
    }

    void CreatePopup(string type, int pointChange)
    {
        float pw = 200f;
        float ph = 50f;
        Vector2 spawnPosition = new Vector2(Random.Range(currentComboText.transform.position.x - pw, currentComboText.transform.position.x + pw),
                                            Random.Range(currentComboText.transform.position.y - ph, currentComboText.transform.position.y + ph));
        if (ui) ui.SpawnPopup(spawnPosition, type, pointChange, transform);
    }
}
