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
    public Transform spawnLocationMenu, spawnLocationHospital, spawnLocationLivingroom;
    public GameObject menuTeleportArea;

    [Header("XRPlayer1 Body Materials")]
    public Material[] playerMaterials1;

    void Awake()
    {
        Instantiate(xrPlayers[0], spawnLocationMenu.position, spawnLocationMenu.rotation);
        Instantiate(menuTeleportArea, new Vector3 (0f, 0f, -0.5f), Quaternion.identity);
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

    public void PatientPlayerSurgeryStageChange (int id)
    {
        if (GameObject.FindGameObjectWithTag("Player").name.Contains("Patient1"))
        {
            Debug.Log("Patient1 player found");
            GameObject.FindGameObjectWithTag("Body").GetComponent<SkinnedMeshRenderer>().material = playerMaterials1[id];
        }
        else if (GameObject.FindGameObjectWithTag("Player").name == "XR Player_Patient2")
        {
            Debug.Log("Patient2 player found but no materials setup");
        }
        else if (GameObject.FindGameObjectWithTag("Player").name == "XR Player_Patient3")
        {
            Debug.Log("Patient3 player found but no materials setup");
        }
        else if (GameObject.FindGameObjectWithTag("Player").name == "XR Player_Patient4")
        {
            Debug.Log("Patient4 player found but no materials setup");
        }
        else
        {
            Debug.Log("PNo patient player found and no materials setup");
        }
    }

    public void ScreenFadeOutIn ()
    {
        GameObject spawnedPlayer = GameObject.FindGameObjectWithTag("Player");
        spawnedPlayer.GetComponentInChildren<SyncVRScreenFade>().FadeOut();
        spawnedPlayer.GetComponentInChildren<SyncVRScreenFade>().FadeIn(); 
    }

    public void ScreenFadeIn ()
    {
        GameObject spawnedPlayer = GameObject.FindGameObjectWithTag("Player");
        spawnedPlayer.GetComponentInChildren<SyncVRScreenFade>().FadeIn();
    }

    public void ScreenFadeOut ()
    {
        GameObject spawnedPlayer = GameObject.FindGameObjectWithTag("Player");
        spawnedPlayer.GetComponentInChildren<SyncVRScreenFade>().FadeOut();
    }

    public void SceneChanged (int id)
    {
        if (id == 0)
        {
            newSpawnedPlayer.transform.position = spawnLocationMenu.position;
            newSpawnedPlayer.transform.rotation = spawnLocationMenu.rotation;
        }
        else if (id == 1)
        {
            newSpawnedPlayer.transform.position = spawnLocationHospital.position;
            newSpawnedPlayer.transform.rotation = spawnLocationHospital.rotation;
        }
        else if (id == 2)
        {
            newSpawnedPlayer.transform.position = spawnLocationLivingroom.position;
            newSpawnedPlayer.transform.rotation = spawnLocationLivingroom.rotation;
        }
        else
        {
            newSpawnedPlayer.transform.position = spawnLocationMenu.position;
            newSpawnedPlayer.transform.rotation = spawnLocationMenu.rotation;
        }
    }
}
