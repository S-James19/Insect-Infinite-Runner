using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Players : MonoBehaviour
{
    public static Players Instance;
    public List<GameObject> players { get; private set; } // list storing players
    private static int PlayerLayer = 6; // layer of player
    private void Awake()
    {
        if(Instance != null && Instance != this) // if there is another use of the script in the scene
        {
            Destroy(gameObject); // destroy the gameobject
        }
        else Instance = this; // this is the instance

        players = new List<GameObject>(); // create a new list to store players
        UpdatePlayers(); // update playercount
    }

    /// <summary>
    /// Function to update the player count in game
    /// </summary>
    private void UpdatePlayers() // function to get all players into a list
    {
        foreach(Transform child in gameObject.transform) // for each player
        {
            if(child.gameObject.layer == PlayerLayer) // if they are a player
            {
                if (!players.Contains(child.gameObject)) // if list does not already contain them
                {
                    players.Add(child.gameObject); // add them to list
                }
            }
        }
    }

    /// <summary>
    /// Function to remove a player from the list of players in the game
    /// </summary>
    /// <param name="player"> The gameobject of the player </param>
    public void RemovePlayer(GameObject player) // function to remove player
    {
        if(players.Contains(player)) // if the list contains the player
        {
            players.Remove(player); // remove the player
        }
    }

    /// <summary>
    /// Function to calculate the closest player from a point
    /// </summary>
    /// <param name="origin"> The point of origin</param>
    /// <returns> A player closest to the point of origin </returns>
    public GameObject ClosestPlayer(GameObject origin)
    {
        float closestPlayerDistance = 1000000f;
        GameObject closestPlayer = null;

        foreach(GameObject player in players) // for each player in list
        {
            float distance = Distance.DistanceBetweenPoints(player.transform.position, origin.transform.position); // calculate distance from origin
            if(distance < closestPlayerDistance) // if they are closer than the last player
            {
                closestPlayer = player; // set the closest player to be that player
                closestPlayerDistance = distance; // set the distance to be the closest distance
            }
        }
        return closestPlayer; // return the player
    }
}
