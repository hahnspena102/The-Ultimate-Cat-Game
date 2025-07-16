using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    [SerializeField] private string catName;
    [SerializeField] private string breed;
    [SerializeField] private int points;
    [SerializeField] private int difficulty;
    [SerializeField] private int coins;
    [SerializeField] private List<Stat> stats;
    [SerializeField] private int appetite, maxAppetite;

    public GameData()
    {
        this.catName = "name";
        this.breed = "breed";
        this.points = 0;
        this.difficulty = 1;
        this.coins = 0;

        this.stats = new List<Stat>()
        {
            new Stat ("Love", 750),
            new Stat ("Hunger", 1000),
            new Stat ("Thirst", 600),
            new Stat ("Energy", 1600),
            new Stat ("Clean", 2000),
            new Stat ("Cozy", 100000),
            new Stat ("Health", 100000),
            new Stat ("Soul", 100000),
            new Stat ("Lifeforce", 100000),
        };

        this.MaxAppetite = 100;
        this.Appetite = this.MaxAppetite;
    }

    public override string ToString()
    {
        string statsString = string.Join("\n  ", stats);
        return $"GameData(\n" +
               $"  CatName: {catName},\n" +
               $"  Breed: {breed},\n" +
               $"  Points: {points},\n" +
               $"  Difficulty: {difficulty},\n" +
               $"  Coins: {coins},\n" +
               $"  Stats:\n  {statsString}\n)";
    }

    public Stat GetStat(string statName)
    {
        foreach (var stat in stats)
        {
            if (stat.Name == statName)
            {
                return stat;
            }
        }
        return null;
    }

    public void UpdateStat(string statName, int delta)
    {
        foreach (var stat in stats)
        {
            if (stat.Name == statName)
            {
                stat.Value += delta;
                return;
            }
        }
    }

    public global::System.String CatName { get => catName; set => catName = value; }
    public global::System.String Breed { get => breed; set => breed = value; }
    public global::System.Int32 Points { get => points; set => points = value; }
    public global::System.Int32 Difficulty { get => difficulty; set => difficulty = value; }
    public global::System.Int32 Coins { get => coins; set => coins = value; }
    public List<Stat> Stats { get => stats; set => stats = value; }
    public global::System.Int32 Appetite { get => appetite; set => appetite = value; }
    public global::System.Int32 MaxAppetite { get => maxAppetite; set => maxAppetite = value; }
}
