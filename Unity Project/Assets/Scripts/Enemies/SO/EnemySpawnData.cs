using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Spawn", menuName = "Scriptable Objects / Enemy Spawn")]
public class EnemySpawnData : ScriptableObject
{
    public Vector3 SpawnPoint;
    public GameObject Prefab;
    public bool Sides;
}
