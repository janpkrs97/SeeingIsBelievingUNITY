using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("XR Player Avatar ID")]
    public int playerID;

    [Header("XR Player GameObjects")]
    public GameObject[] xrPlayers;
    
    [Header("XR Player Spawn Location")]
    public Transform spawnLocation;

    void Awake()
    {
        Instantiate(xrPlayers[0], spawnLocation.position, spawnLocation.rotation);
    }

    public void DestroyPlayers()
    {
        GameObject[] spawnedPlayers = GameObject.FindGameObjectsWithTag("Player");
        
        foreach (GameObject players in spawnedPlayers)
        {
            Destroy(players);
        }
    }

    public void SpawnAvatar (int id)
    {
        Instantiate(xrPlayers[id], spawnLocation.position, spawnLocation.rotation);
        playerID = id;
    }
}
