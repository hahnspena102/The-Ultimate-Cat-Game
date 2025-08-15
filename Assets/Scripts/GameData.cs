using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    [SerializeField] private string catName;
    [SerializeField] private string breed;
    [SerializeField] private int points;
    [SerializeField] private int level;
    [SerializeField] private int coins;
    [SerializeField] private bool isAlive;
    [SerializeField] private List<Stat> stats;
    [SerializeField] private List<bool> upgrades;
    [SerializeField] private int appetite, maxAppetite;
    [SerializeField] private Stat energyRespawn;
    [SerializeField] private float cleanTimer;
    [SerializeField] private int cleanPhase;
    [SerializeField] private float cleanX;
    [SerializeField] private List<CozySavedBall> cozySavedBalls;
    [SerializeField] private List<bool> cozyBonus;
    [SerializeField] private float soulX, soulY;
    [SerializeField] private Stat soulBullets;

    public GameData()
    {
        this.catName = "Meowser";
        this.breed = "breed";
        this.points = 0;
        this.level = 1;
        this.coins = 0;
        this.isAlive = false;

        this.stats = new List<Stat>()
        {
            new Stat ("Love", 750),
            new Stat ("Hunger", 1000),
            new Stat ("Thirst", 600),
            new Stat ("Energy", 1600),
            new Stat ("Clean", 2000),
            new Stat ("Cozy", 2400),
            new Stat ("Health", 3000),
            new Stat ("Soul", 4000),
            new Stat ("Lifeforce", 5000),
        };

        this.upgrades = new List<bool>();
        for (int i = 0; i < 9 * 9; i++)
        {
            this.upgrades.Add(false);
        }


        this.MaxAppetite = 100;
        this.Appetite = this.MaxAppetite;
        this.EnergyRespawn = new Stat("EnergyRespawn", 15);
        this.CozySavedBalls = new List<CozySavedBall>();
        this.cozyBonus = new List<bool> { false, false, false, false, false, false, false, false };
        this.soulBullets = new Stat("SoulBullets", 7);


    }

    public override string ToString()
    {
        string statsString = string.Join("\n  ", stats);
        return $"GameData(\n" +
               $"  CatName: {catName},\n" +
               $"  Breed: {breed},\n" +
               $"  Points: {points},\n" +
               $"  Level: {level},\n" +
               $"  Coins: {coins},\n" +
               $"  IsAlive: {isAlive},\n" +
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
                stat.Update(delta);
                return;
            }
        }
    }

    public bool GetIsLocked(string statName)
    {
        foreach (var stat in stats)
        {
            if (stat.Name == statName)
            {
                return stat.Locked;

            }
        }
        return false;
    }

    public void UpdatePoints(int delta)
    {
        points += Mathf.Max(delta, 0);
        coins += Mathf.FloorToInt(Mathf.Max((float)delta / 10f, 0));
    }

    private float thresholdMultiplier = 1000;
    private float thresholdPower = 1.1f;
    public void UpdateLevel()
    {
        this.level = 1 + Mathf.FloorToInt(Mathf.Pow(points / thresholdMultiplier, 1f / thresholdPower));
    }

    public int GetNextThreshold(float b = -1)
    {
        if (b == -1) b = (float)this.level;

        float res = Mathf.Pow(b, thresholdPower);
        if (b == 0) res = 0;
        return (int)Mathf.Round(thresholdMultiplier * res);
    }

    public global::System.String CatName { get => catName; set => catName = value; }
    public global::System.String Breed { get => breed; set => breed = value; }
    public global::System.Int32 Points { get => points; set => points = value; }
    public global::System.Int32 Level { get => level; set => level = value; }
    public global::System.Int32 Coins { get => coins; set => coins = value; }
    public List<Stat> Stats { get => stats; set => stats = value; }
    public global::System.Int32 Appetite { get => appetite; set => appetite = value; }
    public global::System.Int32 MaxAppetite { get => maxAppetite; set => maxAppetite = value; }
    public Stat EnergyRespawn { get => energyRespawn; set => energyRespawn = value; }
    public global::System.Single CleanTimer { get => cleanTimer; set => cleanTimer = value; }
    public global::System.Int32 CleanPhase { get => cleanPhase; set => cleanPhase = value; }
    public global::System.Single CleanX { get => cleanX; set => cleanX = value; }
    public List<CozySavedBall> CozySavedBalls { get => cozySavedBalls; set => cozySavedBalls = value; }
    public List<global::System.Boolean> CozyBonus { get => cozyBonus; set => cozyBonus = value; }
    public global::System.Single SoulX { get => soulX; set => soulX = value; }
    public global::System.Single SoulY { get => soulY; set => soulY = value; }
    public Stat SoulBullets { get => soulBullets; set => soulBullets = value; }
    public List<global::System.Boolean> Upgrades { get => upgrades; set => upgrades = value; }
    public global::System.Boolean IsAlive { get => isAlive; set => isAlive = value; }
}
