using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateTerrain : MonoBehaviour
{
    [SerializeField] private SpawnData _spawn; 
    [SerializeField] private List<GameObject> _tiles; 
    [SerializeField] private int _maxTiles; 
    [SerializeField] private Material _mudGround, _grassGround, _snowGround;


    private GameObject _startPoint; 
    private Queue<GameObject> _runway = new Queue<GameObject>(); 
    private Vector3 _nextTilePosition;  
    private Vector3 _nextTileSpawn; 
    private Vector3 _initialTileSpawn; 
    private float _maxDistance; 
    private float _lastTileSize; 
    private static int _spawnLayer = 8;
    private bool _isWeatherSetup;
    private void Start()
    {
        _startPoint = Instantiate(_spawn.SpawnPlatform, _spawn.SpawnPoint, Quaternion.identity); // create spawnpoint platform
        _lastTileSize = _startPoint.transform.localScale.z; // last tile size is the starting point
        _nextTilePosition = _startPoint.transform.position + new Vector3(0f, 0f, _startPoint.transform.localScale.z); // place next tile at this point

        // generate maxtiles and spawn infront of player
        for (int i = 0; i < _maxTiles; i++) // while less than maxtiles
        {
            GenerateRandomTile(); // generate a random tile
        }

        _maxDistance = _runway.Peek().transform.localScale.z * 1.5f; // set the base distance away from spawn tile

        transform.position = _startPoint.transform.position + _spawn.SpawnOffset; // setting starting position of player
    }

    private void Update()
    {
        if(!_isWeatherSetup) // if the weather has not been setup yet
        {
            foreach(GameObject tile in _runway) // foreach tile
            {
                ModifyGroundColor(tile); // set the ground color
            }
            _isWeatherSetup = true; // weather has been setup
        }

        if(_runway.Count > 0) // if there are tiles on the runway
        {
            if (Distance.DistanceBetweenPoints(transform.position, _runway.Peek().transform.position) > _maxDistance) // if the player gets too far away from the furthest tile
            {
                Destroy(_runway.Dequeue()); // destroy last tile
                GenerateRandomTile(); // generate a new tile at back of queue
            }
        }
    }
    private void GenerateRandomTile() // function to generate a new tile
    {
        int randomTile = 0; // storing tile index in list
        int randomNumber = 0; // storing number to compare to chance
        bool tileFound = false;

        if(_tiles.Count > 0) // if there are tiles to generate
        {
            while (!tileFound) // while a chosen tile has not been found
            {
                randomTile = Random.Range(0, _tiles.Count); // choose a random tile piece
                randomNumber = Random.Range(0, 100); // chose a random number between 1-100

                if (randomNumber < _tiles[randomTile].GetComponent<Tile>().Chance) // if the random number is less than the chance
                {
                    tileFound = true; // set the found tile to true
                }
            }

            GameObject spawnedTile; // reference tile that will be spawned

            if (_lastTileSize != _tiles[randomTile].transform.localScale.z) // if tile is bigger/smaller than previous tile
            {
                Vector3 modifiedSpawn = new Vector3(0f, 0f, (_tiles[randomTile].transform.localScale.z - _lastTileSize) / 2); // offest tile
                spawnedTile = Instantiate(_tiles[randomTile], _nextTilePosition + modifiedSpawn, Quaternion.identity); // create tile object
                _nextTilePosition += new Vector3(0f, 0f, spawnedTile.transform.localScale.z + modifiedSpawn.z); // set position for next tile
            }
            else // same size
            {
                spawnedTile = Instantiate(_tiles[randomTile], _nextTilePosition, Quaternion.identity); // create tile object
                _nextTilePosition += new Vector3(0f, 0f, spawnedTile.transform.localScale.z); // set position for next tile
            }

            // make child of level gameobject
            if (GameObject.FindGameObjectWithTag("Level"))
            {
                spawnedTile.transform.parent = GameObject.FindGameObjectWithTag("Level").transform;
            }

            if(_isWeatherSetup) // if the weather has been set up
            {
                ModifyGroundColor(spawnedTile); // change the ground color
            }

            _runway.Enqueue(spawnedTile); // add tile to queue
            _lastTileSize = spawnedTile.transform.localScale.z; // set size of last tile */
        }
    }

    /// <summary>
    /// Function to change the material of the tile depending on the weather condition
    /// </summary>
    /// <param name="tile"> The tile to be modified </param>
    private void ModifyGroundColor(GameObject tile)
    {
        Material mat = null;
        if(WeatherSettings.Instance._weatherType == WeatherSettings.WeatherType.rain || WeatherSettings.Instance._weatherType == WeatherSettings.WeatherType.clouds) // if it is raining or cloudy
        {
            mat = _mudGround; // set the material to be mud
        }
        else if(WeatherSettings.Instance._weatherType == WeatherSettings.WeatherType.snow) // if it is snowing
        {
            mat = _snowGround; // set the material to snow
        }
        else mat = _grassGround; // default to grass

        ChangeMaterial(tile, mat); // change the material of the floor

        foreach(Transform child in tile.transform) // for each child of the tile
        {
            if(child.CompareTag("Ground")) // if the tile is also a ground
            {
                ChangeMaterial(child.gameObject, mat); // change the material of the gameobject too
            }
        } 
    }

    /// <summary>
    /// Function to change the material of a tile
    /// </summary>
    /// <param name="tile"> The tile to be changed </param>
    /// <param name="mat"> The material to apply </param>
    private void ChangeMaterial(GameObject tile, Material mat)
    {
        MeshRenderer mesh = tile.GetComponent<MeshRenderer>(); // get the mesh of the tile
        Material[] materials = mesh.materials; // get the materials of that mesh
        materials[0] = mat; // set the material to the new material
        mesh.materials = materials; // set the mesh materials array to modified array.
    }
}

