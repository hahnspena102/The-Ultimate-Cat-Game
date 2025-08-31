using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Values", menuName = "Scriptable Objects/Values")]
public class Values : ScriptableObject
{
    [Header("Stats")]
    
    [SerializeField] private List<int> loveMaxValues = new List<int> { 0, 0, 0, 0};
    [SerializeField] private List<int> hungerMaxValues = new List<int> { 0, 0, 0};
    [SerializeField] private List<int> thirstMaxValues = new List<int> { 600, 1200, 2400, 4800 };
    [SerializeField] private List<int> energyMaxValues = new List<int> { 600, 1200};
    [SerializeField] private List<int> energyRespawnMaxValues = new List<int> { 15, 10, 5 };
    [SerializeField] private List<int> cleanMaxValues = new List<int> { 2000, 4000, 8000 };
    [SerializeField] private List<int> cozyMaxValues = new List<int> { 2400, 4800, 9600, 12000 };
    [SerializeField] private List<int> healthMaxValues = new List<int> { 3000, 5000, 8000, 15000 };
        [SerializeField] private List<int> soulMaxValues = new List<int> { 4000, 8000, 12000, 16000 };
    [SerializeField] private List<int> lifeforceMaxValues = new List<int> { 5000, 10000, 15000, 20000 };
    [ReadOnly] public int LoveMaxValue;
    
    [ReadOnly]public int HungerMaxValue;
    [ReadOnly] public int ThirstMaxValue;
    [ReadOnly] public int EnergyMaxValue;
    [ReadOnly] public int EnergyRespawnMaxValue;
    [ReadOnly] public int CleanMaxValue;
    [ReadOnly] public int CozyMaxValue;
    [ReadOnly] public int HealthMaxValue;
    [ReadOnly] public int SoulMaxValue;
    [ReadOnly] public int LifeforceMaxValue;

    [Header("Love")]

    [SerializeField] private List<int> loveHeartValues = new List<int> { 0, 0, 0, 0 };
    [SerializeField] private List<int> loveCoinValues = new List<int> { 0, 0 };
    [SerializeField] private List<float> loveCanvasXPaddings = new List<float> { 0, 0 };
    [SerializeField] private List<float> loveCanvasYPaddings = new List<float> { 0, 0 };
    [SerializeField] private List<float> loveHeartSizeMultipliers = new List<float> { 0, 0, 0, 0 };
    [SerializeField] private List<float> loveLimitMultipliers = new List<float> { 0, 0};
    [SerializeField] private List<float> loveHeartCounts = new List<float> { 1, 2, 3};

    [ReadOnly] public int LoveHeartValue;
    [ReadOnly]public int LoveCoinValue;
    [ReadOnly]public float LoveCanvasXPadding;
    [ReadOnly]public float LoveCanvasYPadding;
    [ReadOnly]public float LoveHeartSizeMultiplier;
    [ReadOnly]public float LoveLimitMultiplier;
    [ReadOnly]public float LoveHeartCount;

    [Header("Hunger")]

    [SerializeField] private List<int> appetiteMaxValues = new List<int> { 0, 0, 0};
    [SerializeField] private List<int> goodFoodMultipliers = new List<int> { 0, 0};
    [SerializeField] private List<int> badFoodMultipliers = new List<int> { 0, 0};
    [ReadOnly] public int AppetiteMaxValue;
    [ReadOnly] public int GoodFoodMultiplier;
    [ReadOnly] public int BadFoodMultiplier;

    [Header("Thirst")]

    [SerializeField] private List<int> waterDropUpperBounds = new List<int> { 0, 0, 0, 0};
    [SerializeField] private List<int> waterDropLowerBounds = new List<int> { 0, 0};

    [SerializeField] private List<int> thirstPlayerSpeeds = new List<int> { 0, 0};
    
    [ReadOnly] public int WaterDropUpperBound;
    [ReadOnly] public int WaterDropLowerBound;
    [ReadOnly] public int ThirstPlayerSpeed;

    [Header("Energy")]

    [SerializeField] private List<int> energyParticleValues = new List<int> { 0, 0, 0};
    [SerializeField] private List<float> energyParticleScales = new List<float> { 0, 0, 0};
    [SerializeField] private List<int> energyPassiveValues = new List<int> { 0, 0};
    [SerializeField] private List<float>  energyPlayerVerticalSpeeds = new List<float>  { 0, 0};
    [SerializeField] private List<float>  energyPlayerHorizontalSpeeds = new List<float>  { 0, 0};
    
    [ReadOnly] public int EnergyParticleValue;
    [ReadOnly] public float EnergyParticleScale;
    [ReadOnly] public int EnergyPassiveValue;
    [ReadOnly] public float EnergyPlayerHorizontalSpeed;
    [ReadOnly] public float EnergyPlayerVerticalSpeed;

    [Header("Clean")]
    [SerializeField] private List<float> redLightUppers = new List<float> { 0, 0};
    [SerializeField] private List<float> redLightLowers = new List<float> { 0, 0};
    [SerializeField] private List<float> greenLightUppers = new List<float> { 0, 0};
    [SerializeField] private List<float> greenLightLowers = new List<float> { 0, 0};
    [SerializeField] private List<float> yellowLights = new List<float> { 0, 0};
    [SerializeField] private List<int> cleanValues = new List<int> { 0, 0, 0};
    [SerializeField] private List<float> cleanPlayerBoosts = new List<float> { 0, 0, 0, 0};
    [ReadOnly] public float RedLightUpper;
    [ReadOnly] public float RedLightLower;
    [ReadOnly] public float GreenLightUpper;
    [ReadOnly] public float GreenLightLower;
    [ReadOnly] public float YellowLight;
    [ReadOnly] public int CleanValue;
    [ReadOnly] public float CleanPlayerBoost;

    [Header("Cozy")]
    [SerializeField] private List<float> cozyCooldowns = new List<float> { 0, 0,0 ,0};
    [SerializeField] private List<int> maxCozyBalls = new List<int> { 0, 0, 0, 0 };

    [SerializeField] private List<int> cozyValues = new List<int> { 0, 0, 0 };
    [SerializeField] private List<float> cozyMultipliers = new List<float> {0f, 0f, 0 };

    [ReadOnly] public float CozyCooldown;
    [ReadOnly] public int MaxCozyBall;
    [ReadOnly] public int CozyValue;
    [ReadOnly] public float CozyMultiplier;

    [Header("Health")]
    [SerializeField] private List<int> healthCompletionBonuses = new List<int> { 0, 0, 0, 0};
    [SerializeField] private List<int> healValues = new List<int> { 0, 0, 0, 0 };
    [SerializeField] private List<float> healthCoinOdds = new List<float> { 0, 0, 0 };
    [SerializeField] private List<float> healthPathogenOdds = new List<float> { 0, 0, 0, 0 };

    [ReadOnly] public int HealthCompletionBonus;
    [ReadOnly] public int HealValue;
    [ReadOnly] public int HurtValue = -200;
    [ReadOnly] public List<float> HealthOdds = new List<float> {20f, 30f, 50f, 0 };

    [Header("Soul")]
    [SerializeField] private List<int> soulValues = new List<int> { 0, 0, 0, 0 };
    [SerializeField] private List<float>  reloadRates = new List<float> { 0, 0 };
    [SerializeField] private List<int>  maxAmmos = new List<int> { 0, 0, 0 };

    [ReadOnly] public int SoulValue;
    [ReadOnly] public float ReloadRate;
    [ReadOnly] public int MaxAmmo;







    public void UpdateUpgradeValues(List<bool> upgrades)
    {
        if (upgrades.Count < 81) return;




        // LOVE
        // Amiable Affection 1-3
        LoveHeartValue = GetValue(0, loveHeartValues, upgrades);
        LoveHeartSizeMultiplier = GetValue(0, loveHeartSizeMultipliers, upgrades);
        LoveMaxValue = Mathf.RoundToInt(GetValue(0, loveMaxValues, upgrades) * LoveLimitMultiplier);

        LoveHeartCount = GetValue(5, loveHeartCounts, upgrades);

        // Amorous Assets
        LoveCoinValue = GetValue(3, loveCoinValues, upgrades);

        // Love Limit
        LoveCanvasXPadding = GetValue(4, loveCanvasXPaddings, upgrades);
        LoveCanvasYPadding = GetValue(4, loveCanvasYPaddings, upgrades);
        LoveLimitMultiplier = GetValue(4, loveLimitMultipliers, upgrades);

        // HUNGER
        // Chomping Capacity 1-2
        HungerMaxValue = GetValue(12, hungerMaxValues, upgrades);
        AppetiteMaxValue = GetValue(12, appetiteMaxValues, upgrades);

        // Verified Value
        GoodFoodMultiplier = GetValue(16, goodFoodMultipliers, upgrades);
        BadFoodMultiplier = GetValue(16, badFoodMultipliers, upgrades);

        // THIRST
        // Thirstiness Threshold 1-3
        ThirstMaxValue = GetValue(18, thirstMaxValues, upgrades);
        WaterDropUpperBound = GetValue(18, waterDropUpperBounds, upgrades);

        // Purified Precipitation
        WaterDropLowerBound = GetValue(23, waterDropLowerBounds, upgrades);

        // Advanced Agility
        ThirstPlayerSpeed = GetValue(21, thirstPlayerSpeeds, upgrades);

        // ENERGY
        // Particle Production 1-3
        EnergyParticleValue = GetValue(27, energyParticleValues, upgrades);
        EnergyParticleScale = GetValue(27, energyParticleScales, upgrades);

        // Restful Respawn
        EnergyRespawnMaxValue = GetValue(30, energyRespawnMaxValues, upgrades);

        // Everlasting Energy
        EnergyMaxValue = GetValue(34, energyMaxValues, upgrades);
        EnergyParticleValue *= Mathf.RoundToInt(upgrades[34] ? 2f : 1f);
        EnergyPassiveValue = GetValue(34, energyPassiveValues, upgrades);

        // Flgith Finesse
        EnergyPlayerHorizontalSpeed = GetValue(32, energyPlayerHorizontalSpeeds, upgrades);
        EnergyPlayerVerticalSpeed = GetValue(32, energyPlayerVerticalSpeeds, upgrades);

        // CLEAN
        CleanMaxValue = GetValue(39, cleanMaxValues, upgrades);

        RedLightUpper = GetValue(41, redLightUppers, upgrades);
        RedLightLower = GetValue(41, redLightLowers, upgrades);
        GreenLightUpper = GetValue(41, greenLightUppers, upgrades);
        GreenLightLower = GetValue(41, greenLightLowers, upgrades);
        CleanValue = GetValue(39, cleanValues, upgrades);
        CleanPlayerBoost = GetValue(36, cleanPlayerBoosts, upgrades);

        YellowLight = GetValue(43, yellowLights, upgrades);

        // COZY
        CozyMaxValue = GetValue(45, cozyMaxValues, upgrades);

        CozyCooldown = GetValue(45, cozyCooldowns, upgrades);
        MaxCozyBall = GetValue(45, maxCozyBalls, upgrades);

        CozyValue = GetValue(50, cozyValues, upgrades);
        CozyMultiplier = GetValue(50, cozyMultipliers, upgrades);


        // Health
        // Improved Immunity 1-3
        HealthMaxValue = GetValue(54, healthMaxValues, upgrades);
        HealthCompletionBonus = GetValue(54, healthCompletionBonuses, upgrades);
        HealValue = GetValue(54, healValues, upgrades);

        // Preventative Procedure && Profit Pathogen
        HealthOdds[1] = GetValue(57, healthPathogenOdds, upgrades);
        HealthOdds[3] = GetValue(58, healthCoinOdds, upgrades);

        // Soul
        // Spirit Strength 1-3
        SoulMaxValue = GetValue(63, soulMaxValues, upgrades);
        SoulValue = GetValue(63, soulValues, upgrades);

        // Reliable Reload
        ReloadRate = GetValue(67, reloadRates, upgrades);

        // Phantom Pump
        if (upgrades[70])
        {
            MaxAmmo = maxAmmos[2];
        }
        else if (upgrades[66])
        {
            MaxAmmo = maxAmmos[1];
        } else
        {
            MaxAmmo = maxAmmos[0];
        }
        

        // Ammo Athlete
        


    }

    private T GetValue<T>(int startingIndex, List<T> listOfValues, List<bool> upgrades)
    {
        T result = listOfValues[0];

        for (int i = listOfValues.Count - 1; i >= 0; i--)
        {
            int upgradeIndex = startingIndex + i - 1;

            if (upgradeIndex >= 0 && upgradeIndex < upgrades.Count && upgrades[upgradeIndex])
            {
                result = listOfValues[i];
                break;
            }
        }

        return result;
    }

}
