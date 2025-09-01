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
    [SerializeField] private FoodCombo currentFoodCombo, nextFoodCombo;
    [SerializeField] private float foodTimer;
    [SerializeField] private Stat appetite;
    [SerializeField] private Stat energyRespawn;
    [SerializeField] private float cleanTimer;
    [SerializeField] private int cleanPhase;
    [SerializeField] private float cleanX;
    [SerializeField] private List<CozySavedBall> cozySavedBalls;
    [SerializeField] private List<bool> cozyBonus;
    [SerializeField] private float cozyBonusTimer;
    [SerializeField] private float soulX, soulY;
    [SerializeField] private Stat soulBullets;
     [SerializeField] private List<SavedSoulGhost> savedSoulGhosts;

    public GameData()
    {
        this.catName = "";
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
            new Stat ("Energy", 3200),
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

        this.cleanX = -6.3f;
        this.appetite = new Stat("Appetite", 100);
        this.EnergyRespawn = new Stat("EnergyRespawn", 15);
        this.CozySavedBalls = new List<CozySavedBall>();
        this.CozyBonusTimer = 0f;
        this.cozyBonus = new List<bool> { false, false, false, false, false, false, false, false };
        this.soulBullets = new Stat("SoulBullets", 7);
        this.SavedSoulGhosts = new List<SavedSoulGhost>();


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

    public int UpdateStat(string statName, int delta)
    {
        foreach (var stat in stats)
        {
            if (stat.Name == statName)
            {
                return stat.Update(delta);
            }
        }
        return 0;
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
    private float thresholdPower = 1.4f;
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
    public Stat EnergyRespawn { get => energyRespawn; set => energyRespawn = value; }
    public global::System.Single CleanTimer { get => cleanTimer; set => cleanTimer = value; }
    public global::System.Int32 CleanPhase { get => cleanPhase; set => cleanPhase = value; }
    public global::System.Single CleanX { get => cleanX; set => cleanX = value; }
    public List<CozySavedBall> CozySavedBalls { get => cozySavedBalls; set => cozySavedBalls = value; }
    public global::System.Single SoulX { get => soulX; set => soulX = value; }
    public global::System.Single SoulY { get => soulY; set => soulY = value; }
    public Stat SoulBullets { get => soulBullets; set => soulBullets = value; }
    public List<global::System.Boolean> Upgrades { get => upgrades; set => upgrades = value; }
    public global::System.Boolean IsAlive { get => isAlive; set => isAlive = value; }
    public Stat Appetite { get => appetite; set => appetite = value; }
    public List<global::System.Boolean> CozyBonus { get => cozyBonus; set => cozyBonus = value; }
    public global::System.Single CozyBonusTimer { get => cozyBonusTimer; set => cozyBonusTimer = value; }
    public List<SavedSoulGhost> SavedSoulGhosts { get => savedSoulGhosts; set => savedSoulGhosts = value; }
    public FoodCombo CurrentFoodCombo { get => currentFoodCombo; set => currentFoodCombo = value; }
    public FoodCombo NextFoodCombo { get => nextFoodCombo; set => nextFoodCombo = value; }
    public global::System.Single FoodTimer { get => foodTimer; set => foodTimer = value; }
}
