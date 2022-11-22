using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player UI", menuName = "Scriptable Objects / Player UI Data")]
public class PlayerUIData : ScriptableObject
{
    public float Distance { get; set; }
    public float DistanceModifier { get; set; } = 1f;
    public float Speed { get; set; }
}
