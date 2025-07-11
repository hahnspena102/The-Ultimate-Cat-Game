using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField] private DataObject dataObject;
    [SerializeField] private TMPro.TextMeshProUGUI catName;
    [SerializeField] private Slider statMainSlider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (dataObject == null)
        {
            return;
        }

        catName.text = dataObject.PlayerData.GameData.CatName;


        if (statMainSlider)
        {
            Stat curStat = dataObject.PlayerData.GameData.GetStat("Love");
            statMainSlider.value = curStat.Value;
            statMainSlider.maxValue = curStat.MaxValue;
        }



    }
}
