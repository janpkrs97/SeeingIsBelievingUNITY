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
    public GameObject newSpawnedPlayer;
    
    [Header("XR Player Spawn Locations")]
    public Transform spawnLocationMenu, spawnLocationHospital;

    void Awake()
    {
        Instantiate(xrPlayers[0], spawnLocationMenu.position, spawnLocationMenu.rotation);
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
        GameObject[] spawnedPlayers = GameObject.FindGameObjectsWithTag("Player");
        spawnLocationMenu = spawnedPlayers[0].transform;
        newSpawnedPlayer = Instantiate(xrPlayers[id], spawnLocationMenu.position, spawnLocationMenu.rotation);
        GameObject.DontDestroyOnLoad(newSpawnedPlayer);
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

    public void SceneChanged (int id)
    {
        if (id == 0)
        {

        }
        else if (id == 1)
        {
            newSpawnedPlayer.transform.position = spawnLocationHospital.position;
            newSpawnedPlayer.transform.rotation = spawnLocationHospital.rotation;
        }
        else if (id == 2)
        {

        }
        else
        {

        }
    }
}
