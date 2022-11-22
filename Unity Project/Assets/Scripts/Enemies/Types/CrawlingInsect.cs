using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrawlingInsect : FollowEnemy
{
    protected override void PlayerIsHiding(HidingStatus status) // when player is hiding
    {
        if ((status == HidingStatus.left || status == HidingStatus.right) && _isFollowing) // if they are hiding left and right and is being followed
        {
            _isHidden = true; // is hidden

            if(!_isDespawning) // if enemy is not currently despawning
            {
                _isDespawning = true; // they are now going to start despawning
                _despawnStart = Time.time; // set start time of despawn to now
            }
        }
        else // if currently despawning
        {
            //reset boolean values to origional
            _isHidden = false;
            _isDespawning = false;
        }
    }

    protected override void SetupRotation()
    {
        if(transform.position.x > _closestPlayer.transform.position.x) // if the enemy is on the right side of the player
        {
            transform.localRotation = Quaternion.Euler(new Vector3(_rotation.x, -_rotation.y, _rotation.z)); // modifty default rotation value
        }
        else // if enemy is on the left side of the player
        {
            transform.localRotation = Quaternion.Euler(_rotation); // use the default rotation value
        }
    }
}

