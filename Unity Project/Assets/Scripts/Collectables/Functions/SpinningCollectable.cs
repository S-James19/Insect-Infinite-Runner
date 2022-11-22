using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningCollectable : MonoBehaviour
{
    [SerializeField] private float _spinningSpeed = 0.05f;
    [SerializeField] private Direction _direction;

    private float _lastSpinTime;
    public enum Direction // state of direction
    {
        left,
        right
    }

    private void Update()
    {
        if(Time.time > _lastSpinTime + _spinningSpeed) // if time has passed for next spin
        {
            if(_direction == Direction.left) // if the spin direction is left
            {
                transform.Rotate(0f, 0f, -1f, Space.Self); // rotate the local rotation left
            }
            else if(_direction == Direction.right) // if the spin direction is right
            {
                transform.Rotate(0f, 0f, 1f, Space.Self); // rotate the local rotation right
            }

            _lastSpinTime = Time.time; // set the last spin time to now
        }
    }
}
