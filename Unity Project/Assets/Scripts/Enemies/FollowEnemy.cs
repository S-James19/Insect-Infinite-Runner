using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FollowEnemy : BaseEnemy
{
    [SerializeField] protected float _followRange;
    [SerializeField] protected float _attackRange;

    // event to let player know they are being targeted
    public delegate void IsTargeted(float time);
    public static event IsTargeted Target;

    protected float _despawnStart;
    protected bool _isFollowing, _isHidden, _isDespawning;

    /// <summary>
    /// Functionality of enemy when the player is hiding
    /// </summary>
    /// <param name="status"> the hiding position that the player is taking</param>
    protected abstract void PlayerIsHiding(HidingStatus status);

    protected override void Update()
    {
        if(_closestPlayer != null) // if there is a player to target
        {
            base.Update(); // do the origional update first

            float distance = Distance.DistanceBetweenPoints(this.gameObject.transform.position, _closestPlayer.transform.position); // get the distance away from the player

            if(_isDespawning) // if in process of despawning after player is hiding
            {
                if(Time.time > _despawnStart + DespawnManager.Instance._runningTime) // if time has passed
                {
                    Despawn(); // despawn
                }
            }


            if (distance < _followRange && distance > _attackRange) // if enemy is within following range but is not attacking
            {
                if (!_isHidden) // if the player is not hiding
                {
                    Target?.Invoke(DespawnManager.Instance._runningTime); // tell player that they are being targeted
                    _isFollowing = true; // the enemy is following
                    FollowPlayer();
                }
            }
            else if (distance <= _attackRange) // if enemy is within range to attack
            {
                Attack(); // attack player
            }
        }
    }
    private void OnEnable()
    {
        InsectMovement.Hidden += PlayerIsHiding; // subscribe to recieve notification about player hiding status
    }

    private void OnDisable()
    {
        InsectMovement.Hidden -= PlayerIsHiding; // unsubscribe to no longer recieve notification about player hiding status
    }

    /// <summary>
    /// Functionality of enemy when they are following the player
    /// </summary>

    protected virtual void FollowPlayer()
    {
        transform.LookAt(_closestPlayer.transform.position + new Vector3(0f, 0f, _attackRange)); // set rotation to look at the player
        transform.Translate(Vector3.forward * _runningSpeed * Time.deltaTime, Space.Self); // move the gameobject of the enemy in the direction of the player
    }

    /// <summary>
    /// Funcionality of enemy when they are attacking the player
    /// </summary>
    protected override void Attack()
    {
        IDamageable damage = _closestPlayer.GetComponent<IDamageable>(); // get script on player that has functionality to take damage
        damage?.TakeDamage(100); // if script exists, do damage to player
    }

    /// <summary>
    /// Functionaltiy of enemy when they move
    /// </summary>
    protected override void StartMoving()
    {
        if (!_isFollowing) // if the enemy is not following a player
        {
            transform.Translate(Vector3.forward * _runningSpeed * Time.deltaTime, Space.Self); // move in the forward direction
        }
    }
}
