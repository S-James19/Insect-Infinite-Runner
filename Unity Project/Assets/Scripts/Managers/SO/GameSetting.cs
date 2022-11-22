using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Game Setting", menuName = "Scriptable Objects / Game Setting")]
public class GameSetting : ScriptableObject
{
    public float BaseGameSpeed = 1f;
    public float MaxGameSpeed = 3f;
    public float GameSpeed { get; set; }
}
