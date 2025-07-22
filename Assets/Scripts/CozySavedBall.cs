using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using UnityEngine;


[System.Serializable]
public class CozySavedBall
{
    [SerializeField] private float xPos, yPos;
    [SerializeField] private float rotation;
    [SerializeField] private string type;

    public CozySavedBall(float xPos, float yPos, float rotation, string type)
    {
        this.xPos = xPos;
        this.yPos = yPos;
        this.rotation = rotation;
        this.type = type;
    }

    public global::System.Single XPos { get => xPos; set => xPos = value; }
    public global::System.Single YPos { get => yPos; set => yPos = value; }
    public global::System.Single Rotation { get => rotation; set => rotation = value; }
    public global::System.String Type { get => type; set => type = value; }
}
