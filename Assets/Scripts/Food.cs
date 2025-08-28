using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Food
{
    [ReadOnly, SerializeField] private string foodName;
    [ReadOnly, SerializeField] private string type;
    [ReadOnly, SerializeField] private int basePoints;
    [ReadOnly, SerializeField] private string effect;

    public global::System.String FoodName { get => foodName; set => foodName = value; }
    public global::System.String Type { get => type; set => type = value; }
    public global::System.Int32 BasePoints { get => basePoints; set => basePoints = value; }
    public global::System.String Effect { get => effect; set => effect = value; }
}

[System.Serializable]
public class Adjective
{
    [ReadOnly, SerializeField] private string adjectiveName;
    [ReadOnly, SerializeField] private int multiplier;
    [ReadOnly, SerializeField] private string type;

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

    public int Calculate(int goodFoodMultiplier, int badFoodMultiplier)
    {
        int output = 0;

        if (food.Effect == "Good")
        {
            if (adjective.Multiplier >= 0)
            {
                output = adjective.Multiplier * goodFoodMultiplier * food.BasePoints;
            }
            else if (adjective.Multiplier < 0)
            {
                output = adjective.Multiplier * badFoodMultiplier * food.BasePoints;
            }
        }

        else if (food.Effect == "Bad")
        {
            if (adjective.Multiplier >= 0)
            {
                output = -Mathf.Abs(badFoodMultiplier * food.BasePoints);
            }
            else if (adjective.Multiplier < 0)
            {
                output = -Mathf.Abs(adjective.Multiplier * badFoodMultiplier * food.BasePoints);
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

    public Color GetColor(int gluttonGazeLevel, int goodFoodMultiplier, int badFoodMultiplier)
    {
        int pointChange = this.Calculate(goodFoodMultiplier, badFoodMultiplier);
        Color targetColor = new Color(0f, 0f, 0f, 0f);

        if (gluttonGazeLevel >= 1) // Glutton Gaze 1
        {
            if (this.Food.Effect == "Miscellaneous")
            {
                if (gluttonGazeLevel >= 2) targetColor = new Color(0.380f, 0.961f, 0.980f, 1.000f);
            }
            else if (pointChange <= -500)
            {
                targetColor = new Color(0.612f, 0.224f, 0.973f, 1.000f);
            }
            else if (gluttonGazeLevel >= 2) // Glutton Gaze 2
            {
                if (gluttonGazeLevel >= 3) // Glutton Gaze 3
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
        return targetColor;
    }
}