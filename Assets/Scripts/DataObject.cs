using UnityEngine;

[CreateAssetMenu(fileName = "DataObject", menuName = "Scriptable Objects/DataObject")]
public class DataObject : ScriptableObject
{
    [SerializeField] private PlayerData playerData;
    [SerializeField] private string currentStat;
    [SerializeField] private Upgrade selectedUpgrade;

    public PlayerData PlayerData { get => playerData; set => playerData = value; }
    public global::System.String CurrentStat { get => currentStat; set => currentStat = value; }
    public Upgrade SelectedUpgrade { get => selectedUpgrade; set => selectedUpgrade = value; }
}
