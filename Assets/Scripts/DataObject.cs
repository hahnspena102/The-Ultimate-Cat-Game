using UnityEngine;

[CreateAssetMenu(fileName = "DataObject", menuName = "Scriptable Objects/DataObject")]
public class DataObject : ScriptableObject
{
    [SerializeField] private PlayerData playerData;
    [SerializeField] private string currentStat;
    [SerializeField] private Upgrade selectedUpgrade;
    [SerializeField] private bool isPaused;

    public PlayerData PlayerData { get => playerData; set => playerData = value; }
    public global::System.String CurrentStat { get => currentStat; set => currentStat = value; }
    public Upgrade SelectedUpgrade { get => selectedUpgrade; set => selectedUpgrade = value; }
    public global::System.Boolean IsPaused { get => isPaused; set => isPaused = value; }
}
