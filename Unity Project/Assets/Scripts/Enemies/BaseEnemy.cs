using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEnemy : MonoBehaviour
{
    [SerializeField] protected LanesSO _laneData;
    [SerializeField] protected GameSetting _gameSetting;
    [SerializeField] protected float _Speed;
    [SerializeField] protected float _despawnRange;
    [SerializeField] protected float _movingRange;
    [SerializeField] protected EnemySpawnData _spawn;
    [SerializeField] protected Vector3 _rotation;

    protected float _runningSpeed;
    protected bool _isMoving;
    protected GameObject _closestPlayer;


    protected virtual void Start()
    {
        _closestPlayer = Players.Instance.ClosestPlayer(this.gameObject); // get the closest player to the enemy
        SetupRotation();
    }

    /// <summary>
    /// Destroys the enemy gameobject
    /// </summary>
    public virtual void Despawn()
    {
        Destroy(this.gameObject);
    }
    /// <summary>
    /// Attacks the player
    /// </summary>
    protected abstract void Attack();

    /// <summary>
    /// Starts the initial movement of the enemy
    /// </summary>
    protected abstract void StartMoving();

    protected virtual void Update()
    {
        _runningSpeed = _Speed * _gameSetting.GameSpeed; // update speed relative to the game speed

        if (_closestPlayer != null) // if the relative player is not dead
        {
            if (_closestPlayer.transform.position.z > transform.position.z + _despawnRange) // if player has passed enemy by despawn range
            {
                Despawn(); // despawn enemy
            }
            else if (Distance.DistanceBetweenPoints(_closestPlayer.transform.position, this.gameObject.transform.position) < Distance.CalcHypotenuse(_spawn.SpawnPoint) - _movingRange) // if player is within moving range
            {
                _isMoving = true; // enemy is moving
                StartMoving(); // start moving
            }
        }
    }

    /// <summary>
    /// Sets the rotation of the enemy to the specified rotation.
    /// </summary>
    protected virtual void SetupRotation()
    {
        transform.localRotation = Quaternion.Euler(_rotation); // set rotation to given rotation
    }
}
