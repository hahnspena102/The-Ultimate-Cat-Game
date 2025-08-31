using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using UnityEngine;


[System.Serializable]
public class SavedSoulGhost
{
    [SerializeField] private float xPos, yPos;
    [SerializeField] private float xScale;
    [SerializeField] private int health, maxHealth;

    public SavedSoulGhost(float xPos, float yPos, float xScale, int health, int maxHealth = -1)
    {
        this.xPos = xPos;
        this.yPos = yPos;
        this.xScale = xScale;
        this.health = health;
        this.maxHealth = maxHealth;
        if (maxHealth == -1) maxHealth = health;
    }

    public global::System.Single XPos { get => xPos; set => xPos = value; }
    public global::System.Single YPos { get => yPos; set => yPos = value; }
    public global::System.Single XScale { get => xScale; set => xScale = value; }
    public global::System.Int32 Health { get => health; set => health = value; }
    public global::System.Int32 MaxHealth { get => maxHealth; set => maxHealth = value; }
}
