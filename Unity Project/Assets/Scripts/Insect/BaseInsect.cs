using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseInsect : MonoBehaviour, IDamageable, ICollector
{
    [SerializeField] private SpawnData _spawn;
    [SerializeField] private PlayerUIData _data;
    [SerializeField] [Tooltip("Health of insect/player")]
    private float _health = 100f;

    private bool _isDead;
    public float Distance { get; private set; }
    private InsectMovement _movement; // movement script: referencing insect movement 
    private void Awake()
    {
        _data.Distance = Distance; // set the distance of the scriptable object to be 0.
        _data.DistanceModifier = 1f; // set the distance modifier of player to be 1.

        if (GetComponent<InsectMovement>() != null) // if movement script exists
        {
            _movement = GetComponent<InsectMovement>(); // reference movement script
        }
    }

    /// <summary>
    /// Takes damage when being attacked
    /// </summary>
    /// <param name="damage"> The amount of health to take away from the player </param>
    public void TakeDamage(float damage)
    {
        _health -= damage; // remove damage from health

        if(_health <= 0) // if no more health
        {
            _isDead = true; // set player to dead
        }
    }

    /// <summary>
    /// Functionality of player when they are told to die
    /// </summary>

    private void Die()
    {
        Players.Instance.RemovePlayer(gameObject); // remove player from players alive
        Destroy(gameObject); // remove gameobject
    }

    private void Update()
    {
        if(_isDead) // if player is dead
        {
            Die(); // run code to die
            return;
        }

        if(_movement != null) // if movement script exists
        {
            if(!_movement.IsHiding) // if the player is not hiding
            {
                UpdateDistance(); // update distance
            }
        }
    }

    /// <summary>
    /// Updates the distance travelled by the player since origin
    /// </summary>
    private void UpdateDistance() 
    {
        Distance = transform.position.z - _spawn.SpawnPoint.z; // calculate distance from spawn
        _data.Distance = Distance; // update scriptable object to new distance
    }

    /// <summary>
    /// Functionality that is run when the player has interacted with an item
    /// </summary>
    /// <param name="item"> The gameobject that the player has interacted with </param>
    public void PickupItem(GameObject item)
    {
        if(item.CompareTag("Food")) // if the item is food
        {
            _data.DistanceModifier += 0.01f; // update the distance modifier in the player scriptable object
        }
    }
}
