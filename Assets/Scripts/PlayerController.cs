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

    public void IncreasePlayerHeight ()
    {
        GameObject spawnedPlayer = GameObject.FindGameObjectWithTag("Player");
        spawnedPlayer.GetComponentInChildren<CalibratePlayerSize>().GrowHeight();
    }

    public void DecreasePlayerHeight ()
    {
        GameObject spawnedPlayer = GameObject.FindGameObjectWithTag("Player");
        spawnedPlayer.GetComponentInChildren<CalibratePlayerSize>().ShrinkHeight();
    }

    public void IncreasePlayerArms ()
    {
        GameObject spawnedPlayer = GameObject.FindGameObjectWithTag("Player");
        spawnedPlayer.GetComponentInChildren<CalibratePlayerSize>().GrowArms();
    }

    public void DecreasePlayerArms ()
    {
        GameObject spawnedPlayer = GameObject.FindGameObjectWithTag("Player");
        spawnedPlayer.GetComponentInChildren<CalibratePlayerSize>().ShrinkArms();
    }

    public void VisualizeFullBody ()
    {
        GameObject.FindGameObjectWithTag("PlayerLegs").GetComponent<SkinnedMeshRenderer>().enabled = true;
        GameObject[] upperBody = GameObject.FindGameObjectsWithTag("PlayerUpperBody");

        foreach (GameObject obj  in upperBody)
        {
            obj.GetComponent<SkinnedMeshRenderer>().enabled = true;
        }
    }

    public void VisualizeUpperBodyOnly ()
    {
        GameObject.FindGameObjectWithTag("PlayerLegs").GetComponent<SkinnedMeshRenderer>().enabled = false;
        GameObject[] upperBody = GameObject.FindGameObjectsWithTag("PlayerUpperBody");

        foreach (GameObject obj  in upperBody)
        {
            obj.GetComponent<SkinnedMeshRenderer>().enabled = true;
        }
    }

    public void VisualizeFaceOnly ()
    {
        GameObject.FindGameObjectWithTag("PlayerLegs").GetComponent<SkinnedMeshRenderer>().enabled = false;
        GameObject[] upperBody = GameObject.FindGameObjectsWithTag("PlayerUpperBody");

        foreach (GameObject obj  in upperBody)
        {
            obj.GetComponent<SkinnedMeshRenderer>().enabled = false;
        }
    }
}
