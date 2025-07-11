using UnityEngine;

[CreateAssetMenu(fileName = "DataObject", menuName = "Scriptable Objects/DataObject")]
public class DataObject : ScriptableObject
{
    [SerializeField] private PlayerData playerData;

    public PlayerData PlayerData { get => playerData; set => playerData = value; }
}
