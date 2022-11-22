using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Settings", menuName = "Scriptable Objects / Player Setting")]
public class SettingsSO : ScriptableObject
{
    public float TiltSensitivity = 15;
}
