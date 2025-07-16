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
    private int maxReceiptBlocks = 10;

    void Start()
    {
        StartCoroutine(GeneratorCoroutine());
    }

    void Update()
    {
        if (currentComboText) currentComboText.text = currentFoodCombo.ToString();

        if (appetiteSlider && feedButton)
        {
            appetiteSlider.maxValue = dataObject.PlayerData.GameData.MaxAppetite;
            appetiteSlider.value = dataObject.PlayerData.GameData.Appetite;

            float percent = (float)(dataObject.PlayerData.GameData.Appetite / (float)dataObject.PlayerData.GameData.MaxAppetite);

            if (percent < 0.1)
            {
                feedButton.interactable = false;

                ColorBlock colors = appetiteSlider.colors;
                colors.disabledColor = new Color(0.45490196f, 0f, 0.36470588f, 1f);
                appetiteSlider.colors = colors;

            }
            else if (percent == 1)
            {
                feedButton.interactable = true;

                ColorBlock colors = appetiteSlider.colors;
                colors.disabledColor = new Color(0.69803922f, 0.32549020f, 0.86666667f, 1f);

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
        if (dataObject) dataObject.PlayerData.GameData.UpdateStat("Hunger", pointChange);

        if (receiptBlock && receipt)
        {
            GameObject newBlock = Instantiate(receiptBlock, transform.position, Quaternion.identity);
            newBlock.transform.SetParent(receipt);

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
    }
}
