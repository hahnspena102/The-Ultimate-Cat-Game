using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "HungerTable", menuName = "Scriptable Objects/HungerTable")]
public class HungerTable : ScriptableObject
{
    [SerializeField]private List<Food> foods = new List<Food>();
    [SerializeField]private List<Adjective> adjectives = new List<Adjective>();

    public List<Food> Foods { get => foods; set => foods = value; }
    public List<Adjective> Adjectives { get => adjectives; set => adjectives = value; }

    public void Initialize()
    {
        LoadFoodCSV("FoodData");
        LoadAdjectiveCSV("AdjectiveData");
    }

    void LoadFoodCSV(string filename)
    {
        foods = new List<Food>();

        TextAsset fileData = Resources.Load<TextAsset>(filename);
        if (fileData == null)
        {
            Debug.LogError("Food CSV not found!");
            return;
        }

        string[] lines = fileData.text.Split('\n');

        for (int i = 1; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i])) continue;

            string[] values = lines[i].Split(',');

            if (values.Length < 4) continue;

            Food food = new Food
            {
                FoodName = values[0].Trim(),
                Type = values[1].Trim(),
                BasePoints = int.Parse(values[2].Trim()),
                Effect = values[3].Trim()
            };

            foods.Add(food);
        }
    }

    void LoadAdjectiveCSV(string filename)
    {
        adjectives = new List<Adjective>();
        
        TextAsset fileData = Resources.Load<TextAsset>(filename);
        if (fileData == null)
        {
            Debug.LogError("Adjective CSV not found!");
            return;
        }

        string[] lines = fileData.text.Split('\n');

        for (int i = 1; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i])) continue;

            string[] values = lines[i].Split(',');

            if (values.Length < 3) continue;

            Adjective adj = new Adjective
            {
                AdjectiveName = values[0].Trim(),
                Multiplier = int.Parse(values[1].Trim()),
                Type = values[2].Trim()
            };

            adjectives.Add(adj);
        }
    }
}

