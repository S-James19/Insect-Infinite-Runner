using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : IncrementedTime
{
    public delegate void Spawn(GameObject player, Vector3 startingPosition);
    public static event Spawn Spawned;

    [SerializeField] private float _timeBetweenSpawns;
    [SerializeField] private InsectMovement _movement;

    private Vector3 _startingPosition;
    private float _spawnModifier = 1f;
    private float _timeAlongPath, _lastHideTime, _lastSpawnTime;
    private bool _hasHiddenBefore, _gameStarted;

    protected override void Awake()
    {
        base.Awake();
        _gameStarted = false;
    }
    protected override void Update()
    {
        base.Update();
        if(_gameStarted) // if spawner has been setup
        {
            if(_movement != null) // if movement script is present
            {
                if (_movement.InsectHidingStatus != HidingStatus.none) // if player is hiding
                {
                    if (!_hasHiddenBefore)
                    {
                        _timeAlongPath += _runningTime - ((_lastSpawnTime + _runningTime) - Time.time); // 2s along path, timeAlongPath = 2
                        _hasHiddenBefore = true;
                    }

                    _lastSpawnTime = Time.time;
                }
                else _hasHiddenBefore = false;
            }

            if(Time.time + _timeAlongPath > _lastSpawnTime + _runningTime) // if can spawn enemy
            {
                Spawned?.Invoke(this.gameObject, _startingPosition); // tell spawn manager that it can spawn enemy
                _lastSpawnTime = Time.time;
                _timeAlongPath = 0;
            }
        }
        else // if not 
        {
            if(GameManager.Instance.gameState == GameManager.GameState.Playing) // if game has started to play
            {
                _startingPosition = transform.position;
                _lastSpawnTime = Time.time; // set the spawn timer
                _gameStarted = true; // set the game to started
            }
        }
    }
}
