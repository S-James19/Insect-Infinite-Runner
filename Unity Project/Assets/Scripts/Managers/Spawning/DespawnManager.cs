using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnManager : IncrementedTime
{
    public static DespawnManager Instance { get; private set; }

    protected override void Awake()
    {
        if(Instance != null && Instance != this) // if there is another despawnmanager in scene
        {
            Destroy(gameObject); // destroy this version
        }
        else // if not
        {
            Instance = this; // this is the despawn manager
        }
        base.Awake();
    }
}
