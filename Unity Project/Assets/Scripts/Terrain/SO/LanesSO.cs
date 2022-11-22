using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Lane Type", menuName = "Scriptable Objects / New Lane")]
public class LanesSO : ScriptableObject
{
    public int Lanes = 3;
    public float Width = 10f;
}
