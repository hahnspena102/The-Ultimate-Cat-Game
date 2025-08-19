using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class UI : MonoBehaviour
{
    [SerializeField] private DataObject dataObject;
    [SerializeField] private TMPro.TextMeshProUGUI catName;
    [SerializeField] private Slider statMainSlider;
    [SerializeField] private Image statMainImage;
    [SerializeField] private TMPro.TextMeshProUGUI statMainText;
    [SerializeField] private Transform statSummary;
    [SerializeField] private List<Sprite> iconSprites;
    [SerializeField] private GameObject popupPrefab;
    [SerializeField] private TextMeshProUGUI pointTextBox, coinTextBox;
    [SerializeField] private Slider catLevelSlider;
    [SerializeField] private TextMeshProUGUI catLevelText;

    public static Dictionary<string, Sprite> StatToSpriteMap;
    public static Dictionary<string, Color> StatToColorMap;

    void Awake()
    {
        StatToSpriteMap = new Dictionary<string, Sprite>();
        StatToColorMap = new Dictionary<string, Color>();

        string[] statNames = new string[]
        {
            "Love", "Hunger", "Thirst", "Energy",
            "Clean", "Cozy", "Health", "Soul", "Lifeforce"
        };

        List<Color> statColors = new List<Color>()
        {
            new Color(1f, 0.28235294f, 0.47058824f, 1f),
            new Color(0.09803922f, 1f, 0.14117647f, 1f),
            new Color(0f, 0.92549020f, 1f, 1f),
            new Color(1f, 0.58823529f, 0.24313725f, 1f),
            new Color(0.40000000f, 0.63921569f, 1f, 1f),
            new Color(0.70588235f, 0.40000000f, 1f, 1f),
            new Color(0.11764706f, 0.83529412f, 0.59607843f, 1f),
            new Color(0.21568627f, 0.20784314f, 0.26666667f, 1f),
            new Color(1f, 0.96862745f, 0.40000000f, 1f)

        };

        for (int i = 0; i < statNames.Length && i < iconSprites.Count && i < statColors.Count; i++)
        {
            StatToSpriteMap[statNames[i]] = iconSprites[i];
            StatToColorMap[statNames[i]] = statColors[i];
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (dataObject == null)
        {
            return;
        }

        catName.text = dataObject.PlayerData.GameData.CatName;


        if (statMainSlider && statMainImage && statMainText)
        {
            Stat curStat = dataObject.PlayerData.GameData.GetStat(dataObject.CurrentStat);

            if (curStat != null)
            {
                statMainSlider.value = curStat.Value;
                statMainSlider.maxValue = curStat.MaxValue;

                ColorBlock colors = statMainSlider.colors;
                colors.disabledColor = StatToColorMap[dataObject.CurrentStat];
                statMainSlider.colors = colors;

                statMainImage.sprite = StatToSpriteMap[dataObject.CurrentStat];

                statMainText.text = $"{curStat.Value}/{curStat.MaxValue}";
            }
        }

        if (pointTextBox) pointTextBox.text = $"{dataObject.PlayerData.GameData.Points}";
        if (coinTextBox) coinTextBox.text = $"{dataObject.PlayerData.GameData.Coins}";

        if (statSummary)
        {
            foreach (Transform statRow in statSummary)
            {
                string statName = statRow.name;
                Stat stat = dataObject.PlayerData.GameData.GetStat(statName);

                if (stat != null)
                {
                    if (stat.Locked)
                    {
                        CanvasGroup cg = statRow.GetComponent<CanvasGroup>();
                        if (cg) cg.alpha = 0;
                    }
                    else
                    {
                        CanvasGroup cg = statRow.GetComponent<CanvasGroup>();
                        if (cg) cg.alpha = 1;
                        Transform percentageObj = statRow.Find("Percentage");
                        if (percentageObj != null)
                        {
                            TMPro.TextMeshProUGUI percentageText = percentageObj.GetComponent<TMPro.TextMeshProUGUI>();
                            if (percentageText != null)
                            {
                                float percent = (float)stat.Value / (float)stat.MaxValue * 100f;

                                percentageText.text = $"{Mathf.Floor(percent)}%";
                            }
                        }
                    }

                }
            }
        }

        if (catLevelSlider && catLevelText)
        {
            int threshold = dataObject.PlayerData.GameData.GetNextThreshold();
            int prevThreshold = dataObject.PlayerData.GameData.GetNextThreshold(dataObject.PlayerData.GameData.Level - 1);

            catLevelSlider.value = dataObject.PlayerData.GameData.Points - prevThreshold;
            catLevelSlider.maxValue = threshold - prevThreshold;

            catLevelText.text = $"{dataObject.PlayerData.GameData.Level}";
        }
    }

    public GameObject SpawnPopup(Vector3 position, string stat, int number, Transform parent = null)
    {
        GameObject popup = Instantiate(popupPrefab, position, Quaternion.identity, parent);

        if (popup == null)
        {
            return null;
        }

        Popup p = popup.GetComponent<Popup>();

        if (!StatToColorMap.TryGetValue(stat, out Color color))
        {
            color = Color.black;
        }
        p.OutlineColor = color;
        p.Number = number;

        return popup;
    }
    
}
