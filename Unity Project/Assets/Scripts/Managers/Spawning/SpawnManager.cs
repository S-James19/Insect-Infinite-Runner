using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance { get; private set; }
    [SerializeField] private List<EnemySpawnData> _enemies;

    private static int _enemyLayer = 9;

    private void Awake()
    {
        if (Instance != null && Instance != this) // if a previous spawn manager exists
        {
            Destroy(gameObject); // destroy this one
        }
        else Instance = this; // make this the spawn manager reference
    }

    private void OnEnable()
    {
        SpawnEnemy.Spawned += InstantiateEnemy; // subscribe to recieve events about spawning
    }

    private void OnDisable()
    {
        SpawnEnemy.Spawned -= InstantiateEnemy; // unsubscribe to not recieve events about spawning
    }

    /// <summary>
    /// Function to spawn in an enemey
    /// </summary>
    /// <param name="player"> The player in which the enemy will be spawned near</param>
    /// <param name="startingPosition"> The position of the player when wanting to spawn an enemy </param>
    public void InstantiateEnemy(GameObject player, Vector3 startingPosition)
    {
        int randomSide = Random.Range(0, 2); // get random side to spawn on
        int randomEnemy = Random.Range(0, _enemies.Count); // find random enemy to spawn
        EnemySpawnData enemy = _enemies[randomEnemy]; // reference enemy scritpable object

        Vector3 spawnPoint = enemy.SpawnPoint; // set spawn point to eneny spawn point
        Vector3 spawnRotation = new Vector3(0f, 0f, 0f);

        if (enemy.Sides) // if enemy can spawn on multiple sides
        {
            if (randomSide == 1)
            {
                spawnPoint = new Vector3(-spawnPoint.x, spawnPoint.y, spawnPoint.z); // set x coordinate to negative
            }
        }
        GameObject spawned = Instantiate(enemy.Prefab, spawnPoint + new Vector3(startingPosition.x, 0f, player.transform.position.z), Quaternion.identity); // spawn enemy
    }
}
