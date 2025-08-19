using UnityEngine;
using TMPro;

public class StatBlock : MonoBehaviour
{
    [SerializeField] private DataObject dataObject;
    [SerializeField] private string type;
    [SerializeField] private TextMeshProUGUI textMesh;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Stat stat = dataObject.PlayerData.GameData.GetStat(type);

        if (stat.Percent() <= 0.05f)
        {
            textMesh.color = new Color(1.000f, 0.396f, 0.396f, 1.000f);
        }
        else
        {
            textMesh.color = new Color(1.000f, 1f, 1f, 1.000f);
        }

        if (type == dataObject.CurrentStat)
        {
            textMesh.fontStyle |= FontStyles.Underline;
        }
        else
        {
            textMesh.fontStyle &= ~FontStyles.Underline;
        }
            
    }
}
