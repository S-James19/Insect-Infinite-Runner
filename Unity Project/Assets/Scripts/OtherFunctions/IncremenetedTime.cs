using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IncrementedTime : MonoBehaviour
{ 
    [SerializeField] protected GameSetting gameSetting;
    [SerializeField] protected float _defaultTime = 3f;
    [SerializeField] protected float _lowestTime = 1f;
    [SerializeField] protected float _intervals = 0.005f;

    public float _runningTime { get; private set; }
    private float _lastSpeed;

    protected virtual void Awake()
    {
        float time = _defaultTime / gameSetting.BaseGameSpeed; // get the time depending on the game sped

        if (time > _lowestTime) // if the time is bigger than the lowest time
        {
            _runningTime = time; // set the running time to that time
        }
        else _runningTime = _lowestTime; // if it is lower of the same, then set the running time to the lowest time
    }

    private void Start()
    {
        _lastSpeed = gameSetting.GameSpeed; // set the last speed to the current game speec
    }

    protected virtual void Update()
    {
        if (_runningTime > _lowestTime) // if can decrease time
        {
            if (gameSetting.GameSpeed > _lastSpeed) // if the game speed has changed
            {
                _runningTime -= _intervals; // remove one interval from the running time
                _lastSpeed = gameSetting.GameSpeed; // set the last game speed to the current game speed
            }
        }
    }
}
