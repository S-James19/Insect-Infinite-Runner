using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chaser : IncrementedTime
{
    private bool _isHiding;
    private float _hidingTime;
    protected override void Awake()
    {
        base.Awake(); // run the base awake
        InsectMovement.Hidden += WhenHidden; // subscribe to receive information about when the player is hiding 
    }

    private void OnEnable()
    {
        InsectMovement.Hidden += WhenHidden; // subscribe to receive information about when the player is hiding 
    }

    private void OnDisable()
    {
        InsectMovement.Hidden -= WhenHidden; // unsubscribe to no longer receive information about when the player is hiding 
    }

    private void OnDestroy()
    {
        InsectMovement.Hidden -= WhenHidden; // unsubscribe to no longer receive information about when the player is hiding 
    }

    public void WhenHidden(HidingStatus status)
    {
        if (status != HidingStatus.none) // if the player is hiding
        {
            if (!_isHiding) // if the current bool for hiding is false
            {
                _hidingTime = Time.time; // update the start time of hiding to current time
                _isHiding = true; // set hiding status to true.
            }
            else // if already hiding
            {
                if (Time.time > _hidingTime + _runningTime) // if time has passed for chaser to kill player
                {
                    IDamageable health = GetComponent<IDamageable>(); // get the script on player with functionality to do damage
                    health?.TakeDamage(100); // attack the player
                }
            }
        }
        else _isHiding = false; // set player hiding status to false
    }
}
