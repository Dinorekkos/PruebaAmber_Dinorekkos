using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "CannonMultiplier", menuName = "CannonMultiplier")]
public class CannonData : ScriptableObject
{
    public List<CannonLevel> CannonLevels;
    
}

[Serializable]
public class CannonLevel
{
    public int Level;
    public float speedCannon;
    public int Damage;
    public float Range;
    public int UpgradeCost;
}