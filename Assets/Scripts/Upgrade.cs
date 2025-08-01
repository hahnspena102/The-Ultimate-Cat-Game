using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Upgrade", menuName = "Scriptable Objects/Upgrade")]
public class Upgrade : ScriptableObject
{
    [SerializeField] private string upgradeName;
    [SerializeField] private string id;
    [SerializeField] private Sprite sprite;
    [SerializeField] private string description;
    [SerializeField] private int cost;
    [SerializeField] private List<Upgrade> prerequisites = new List<Upgrade>();

    public global::System.String UpgradeName { get => upgradeName; set => upgradeName = value; }
    public global::System.String Id { get => id; set => id = value; }
    public global::System.String Description { get => description; set => description = value; }
    public global::System.Int32 Cost { get => cost; set => cost = value; }
    public List<Upgrade> Prerequisites { get => prerequisites; set => prerequisites = value; }
    public Sprite Sprite { get => sprite; set => sprite = value; }
}
