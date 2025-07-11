using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UI : MonoBehaviour
{
    [SerializeField] private DataObject dataObject;
    [SerializeField] private TMPro.TextMeshProUGUI catName;
    [SerializeField] private Slider statMainSlider;
    [SerializeField] private Image statMainImage;
    [SerializeField] private Transform statSummary;
    [SerializeField] private List<Sprite> iconSprites;
    private Dictionary<string, Sprite> statToSpriteMap;
    private Dictionary<string, Color> statToColorMap;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        statToSpriteMap = new Dictionary<string, Sprite>();
        statToColorMap = new Dictionary<string, Color>();

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
            statToSpriteMap[statNames[i]] = iconSprites[i];
            statToColorMap[statNames[i]] = statColors[i];
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


        if (statMainSlider && statMainImage)
        {
            Stat curStat = dataObject.PlayerData.GameData.GetStat(dataObject.CurrentStat);
            statMainSlider.value = curStat.Value;
            statMainSlider.maxValue = curStat.MaxValue;

            ColorBlock colors = statMainSlider.colors;
            colors.disabledColor = statToColorMap[dataObject.CurrentStat];
            statMainSlider.colors = colors;

            statMainImage.sprite = statToSpriteMap[dataObject.CurrentStat];
        }

        if (statSummary)
        {
            foreach (Transform statRow in statSummary)
            {
                string statName = statRow.name;
                Stat stat = dataObject.PlayerData.GameData.GetStat(statName);

                if (stat != null)
                {
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
}
