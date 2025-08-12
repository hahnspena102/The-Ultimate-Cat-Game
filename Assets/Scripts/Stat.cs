using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat
{
    [SerializeField] private string name;
    [SerializeField] private int value;
    [SerializeField] private int maxValue;
    [SerializeField] private bool locked;

    public string Name { get => name; set => name = value; }
    public int Value { get => value; set => this.value = value; }
    public int MaxValue { get => maxValue; set => maxValue = value; }
    public global::System.Boolean Locked { get => locked; set => locked = value; }

    public Stat(string name, int valueMax)
    {
        this.name = name;
        this.value = valueMax;
        this.maxValue = valueMax;
        this.locked = false;
    }

    public Stat(string name, int value, int valueMax, bool unlocked)
    {
        this.name = name;
        this.value = value;
        this.maxValue = valueMax;
        this.locked = unlocked;
    }

    public override string ToString()
    {
        return $"Stat(Name: {name}, Value: {value}/{maxValue}, Unlocked: {locked})";
    }

    public void Update(int delta)
    {
        if (this.locked) return;
        if (delta >= 0)
        {
            this.Value = Mathf.Min(this.Value + delta, this.MaxValue);
        }
        else
        {
            this.Value = Mathf.Max(this.Value + delta, 0);
        }
    }
}