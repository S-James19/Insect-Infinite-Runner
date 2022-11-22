using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Collectable", menuName = "Scriptable Objects / Collectable")]
public class CollectablesSO : ScriptableObject
{
    public string CollectableName;
    [Range(0, 100)] public int SpawnChance;
    public Vector3 Rotation;
    public GameObject Prefab;
}
