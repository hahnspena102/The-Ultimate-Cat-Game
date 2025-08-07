using UnityEngine;
using TMPro;

public class UpgradeCoins : MonoBehaviour
{
    [SerializeField] private DataObject dataObject;
    [SerializeField] private TextMeshProUGUI textMesh;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        textMesh.text = $"{dataObject.PlayerData.GameData.Coins}";
    }
}
