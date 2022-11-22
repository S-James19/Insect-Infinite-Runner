using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaf : MonoBehaviour
{
    [SerializeField] private CollectablesSO _wormData;
    [SerializeField] private float _despawnDistance = 30f;
    private static int _playerLayer = 6;

    private GameObject _closestPlayer;
    private void Start()
    {
        _closestPlayer = Players.Instance.ClosestPlayer(gameObject); // get the closest player in game
    }

    private void Update()
    {
        if(_closestPlayer != null) // if there are any players in the game
        {
            if ((_closestPlayer.transform.position.z > transform.position.z) && (Distance.DistanceBetweenPoints(_closestPlayer.transform.position, transform.position) > _despawnDistance)) // if player gets too far away
            {
                Destroy(gameObject); // destroy
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == _playerLayer) // if the leaf collides with a player
        {
            other.gameObject.GetComponent<ICollector>()?.PickupItem(gameObject); // if ICollector script exists, run function in response to picking up item.
            Despawn(); // despawn leaf
        }
    }


    /// <summary>
    /// Destroys the leaf gameobject
    /// </summary>
    private void Despawn()
    {
        Destroy(gameObject);
    }
}
