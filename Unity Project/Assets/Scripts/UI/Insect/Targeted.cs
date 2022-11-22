using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Targeted : MonoBehaviour
{
    [SerializeField] private Image _image;
    [Range(0, 1)] [SerializeField] private float _opacity;
    [SerializeField] private float _opacityChangeTime;
    [SerializeField] private float _resetTime;

    private float _despawnTime, _despawnCounter;
    private float _lastTargetTime, lastDespawnTime;
    private float _opacityIncrease = 1f / 255f;
    private bool _isTargeted, _isUntargeting;

    /// <summary>
    /// Function to increase the opacity of the target image when being targeted by an enemy
    /// </summary>
    /// <param name="time"> How long it takes for the enemy to despawn </param>
    private void BeingTargeted(float time)
    {
        if(Time.time > _lastTargetTime + _opacityChangeTime) // if the current time is bigger than the last time of opacity change and time between changes
        {
            if(!_isTargeted) // if they are only just being targeted
            {
                _despawnTime = time; // setup the time of despawn
                _isTargeted = true; // target has been setup
            }

            if(_opacity >= 0 && _opacity < 1) // if opacity can be modifed
            {
                SetNewOpacity(); // modify the opacity
                _opacity += _opacityIncrease; // increase the opacity
                _lastTargetTime = Time.time; // set the time of last change to current time
            }  
        }
    }
    private void Update()
    {
        if(_isTargeted) // if being targeted
        {
            if(Time.time > _lastTargetTime + _resetTime) // time has passed to reset
            {
                if(!_isUntargeting) // if player is not being untargeted
                {
                    float opacityCount = _opacity / _opacityIncrease; // check how much the opacity has increased
                    _despawnCounter = _despawnTime / opacityCount; // check how often to decrease the opacity
                    _isUntargeting = true; // player is now being untargeted
                }
                else // if being untargeted
                {
                    if (Time.time > lastDespawnTime + _despawnCounter) // if time has passed to decrease opacity
                    {
                        if (_opacity > _opacityChangeTime) // if opacity can be decreased
                        {
                            SetNewOpacity(); // change opacity
                            _opacity -= _opacityIncrease; // decrease opacity
                            lastDespawnTime = Time.time; // set last time of change to current time
                        }
                        else // if opacity cannot be decreased
                        {
                            // reset boolean values to origional
                            _isTargeted = false;
                            _isUntargeting = false;
                        }
                    }
                }


            }
        }
    }

    /// <summary>
    /// Function to modify the opacity of the target image
    /// </summary>
    private void SetNewOpacity()
    {
        Color color = _image.color; // get the current color of image
        color.a = _opacity; // set new opacity of image
        _image.color = color; // set image color to modified color.
    }

    private void OnEnable()
    {
        _isTargeted = false; // player is not being targeted
        _lastTargetTime = Time.time; // set last target time to current time
        FollowEnemy.Target += BeingTargeted; // subscribe to get notification when being targeted.
    }

    private void OnDisable()
    {
        FollowEnemy.Target -= BeingTargeted; // unsubscribe to no longer get notification when being targeted.
    }
}
