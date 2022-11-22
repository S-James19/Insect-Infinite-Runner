using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "spawn", menuName = "Scriptable Objects / Spawnpoint")]
public class SpawnData : ScriptableObject
{
    public Vector3 SpawnOffset;
    public Vector3 SpawnPoint;
    public GameObject SpawnPlatform;
}
