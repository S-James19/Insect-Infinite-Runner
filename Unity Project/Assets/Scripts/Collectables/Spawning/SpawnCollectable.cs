using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpawnCollectable : MonoBehaviour
{
    [SerializeField] protected LanesSO _playersLaneData;
    [SerializeField] protected CollectablesSO _collectable;
    [SerializeField] protected int _gapBetweenCollectables = 3;

    protected float _furthestLane, _outsideLanes;
    protected Vector3 _centerLane;

    private void Awake()
    {
        _centerLane = transform.localScale;
        _outsideLanes = (_playersLaneData.Lanes - 1) / 2; // get how many outside lanes either side of center
        _furthestLane = transform.position.x + (_outsideLanes * _playersLaneData.Width); // get x position of furthest lane to the right

        for(int i = 0; i < _playersLaneData.Lanes; i++) // for each lane in the runway
        {
            for(int j = 0; j < transform.localScale.z - _gapBetweenCollectables; j += _gapBetweenCollectables) // for every 3 scale position
            {
                SpawnItem(i, j); // spawn an item
            }

            _furthestLane -= _playersLaneData.Width;
        }
    }

    /// <summary>
    /// Randomly spawns in prefab at speicified positions
    /// </summary>
    /// <param name="lane"> The number of the lane </param>
    /// <param name="position"> The position along the lane's z coordinate </param>
    public abstract void SpawnItem(int lane, int position);

}
