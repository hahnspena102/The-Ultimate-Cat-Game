using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Food
{
    [SerializeField] private string foodName;
    [SerializeField] private string type;
    [SerializeField] private int basePoints;
    [SerializeField] private string effect;

    public global::System.String FoodName { get => foodName; set => foodName = value; }
    public global::System.String Type { get => type; set => type = value; }
    public global::System.Int32 BasePoints { get => basePoints; set => basePoints = value; }
    public global::System.String Effect { get => effect; set => effect = value; }
}

[System.Serializable]
public class Adjective
{
    [SerializeField] private string adjectiveName;
    [SerializeField] private int multiplier;
    [SerializeField] private string type;

    public global::System.String AdjectiveName { get => adjectiveName; set => adjectiveName = value; }
    public global::System.Int32 Multiplier { get => multiplier; set => multiplier = value; }
    public global::System.String Type { get => type; set => type = value; }
}


[System.Serializable]
public class FoodCombo
{
    [SerializeField] private Food food;
    [SerializeField] private Adjective adjective;

    public FoodCombo(Food fd, Adjective adj)
    {
        adjective = adj;
        food = fd;
    }

    public Food Food { get => food; set => food = value; }

    public int Calculate()
    {
        int output = 0;

        if (food.Effect == "Good")
        {
            output = adjective.Multiplier * 20 * food.BasePoints;
        }

        else if (food.Effect == "Bad")
        {
            if (adjective.Multiplier >= 0)
            {
                output = -Mathf.Abs(25 * food.BasePoints);
            }
            else if (adjective.Multiplier < 0)
            {
                output = -Mathf.Abs(adjective.Multiplier * 25 * food.BasePoints);
            }
        }
        else if (food.Effect == "Miscellaneous")
        {
            if (food.FoodName == "Catnip")
            {
                output = 1;
            }
            else if (food.FoodName == "Dog Food")
            {
                output = 50;
            }
            else if (food.FoodName == "Mysterious Essence")
            {
                output = 555;
            }
            else if (food.FoodName == "Mystery Cat Food")
            {
                output = UnityEngine.Random.Range(-555, 555);
            }
            else if (food.FoodName == "Poison")
            {
                output = -555;
            }
            else
            {
                output = 0;
            }
        }

        /*if (DataManager.playerData.satiation > DataManager.playerData.satiationMax && !ignoreSatiation)
        {
            output = -Math.Abs(output);
        }
        */

        return output;
    }
    override public string ToString()
    {
        if (adjective.Type == "None")
        {
            return $"{food.FoodName}";
        }
        return $"{adjective.AdjectiveName} {food.FoodName}";
    }
}