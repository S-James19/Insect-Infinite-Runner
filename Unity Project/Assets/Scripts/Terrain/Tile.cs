using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Vector3 StartingPosition { get; private set; }
    [Range(2, 100)] public int Chance;

    private void Awake()
    {
        StartingPosition = transform.position;
    }
}
