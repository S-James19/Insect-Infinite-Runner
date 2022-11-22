using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLeaves : SpawnCollectable
{
    [SerializeField] private int _leafStreak = 2;
    private bool _wasWorm;
    public override void SpawnItem(int lane, int position)
    {
        int randomNumber = Random.Range(0, 100); // get random number between 1-100

        if ((_wasWorm && randomNumber < _collectable.SpawnChance * _leafStreak) || (!_wasWorm && randomNumber < _collectable.SpawnChance))  // if it meets criteria to spawn
        {
            GameObject spawnedleaf = Instantiate(_collectable.Prefab, new Vector3(_furthestLane, transform.position.y + _collectable.Prefab.transform.localScale.x, transform.position.z + position), Quaternion.identity); // spawn leaf at set position
            spawnedleaf.transform.localRotation = Quaternion.Euler(_collectable.Rotation); // set the rotation of the leaf
            _wasWorm = true; // was a worm, increasing chance of next iteration being a worm
        }
        else _wasWorm = false; // did not spawn anything at position
    }
}
