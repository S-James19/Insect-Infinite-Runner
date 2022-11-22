using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : FollowEnemy
{
    protected override void Start()
    {
        base.Start(); // run the base start
        transform.localRotation = Quaternion.Euler(_rotation); // set the rotation of the bird to be the specified rotation
    }
    protected override void PlayerIsHiding(HidingStatus status) // when player is hiding
    {
        if ((status == HidingStatus.under && _isFollowing)) // if they are hiding left and right and is being followed
        {
            _isHidden = true; // is hidden

            if (!_isDespawning) // if enemy is not despawning
            {
                _isDespawning = true; // they are now despawning
                _despawnStart = Time.time; // set start time of despawn to now
            }
        }
        else // if enemy is currently despawning
        {
            // set boolean values back to origional
            _isHidden = false;
            _isDespawning = false;
        }
    }
}
