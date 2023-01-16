using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayerController : MonoBehaviour
{
    //public static PlayerController instance;

    [Header("XR Player Avatar ID")]
    public int playerID;

    [Header("XR Player GameObjects")]
    public GameObject[] xrPlayers;
    public GameObject newSpawnedPlayer;
    public Transform spawnLocationMenu, spawnLocationHospital, spawnLocationLivingroom;

    [Header("XRPlayer1 Body Materials")]
    public Material[] playerMaterials1;
    public int playerMaterialID;

    void Awake()
    {
        // destroy 2nd manager object when menu scene is reloaded
        /*if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            DestroyImmediate(gameObject);
        }*/
    }

    void Start() 
    {
        //spawnLocationMenu = GetComponentInChildren<Transform>();
        Instantiate(xrPlayers[0], spawnLocationMenu.position, spawnLocationMenu.rotation);
        playerID = 0;
        
        // update teleportation area's interaction manager & teleportation provider
        //GameObject.FindGameObjectWithTag("TeleportArea").GetComponentInChildren<TeleportationArea>().teleportationProvider = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<TeleportationProvider>();
        //GameObject.FindGameObjectWithTag("TeleportArea").GetComponentInChildren<TeleportationArea>().interactionManager = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<XRInteractionManager>();
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
        playerID = id;

        // update teleportation area's interaction manager & teleportation provider
        //GameObject.FindGameObjectWithTag("TeleportArea").GetComponentInChildren<TeleportationArea>().teleportationProvider = newSpawnedPlayer.GetComponentInChildren<TeleportationProvider>();
        //GameObject.FindGameObjectWithTag("TeleportArea").GetComponentInChildren<TeleportationArea>().interactionManager = newSpawnedPlayer.GetComponentInChildren<XRInteractionManager>();

        GameObject.DontDestroyOnLoad(newSpawnedPlayer);
    }

    public void UpdateTeleportationAnchorReferences()
    {
        // update teleportation anchors' interaction managers & teleportation providers
        //GameObject[] tutorialTeleportationAnchors = GameObject.FindGameObjectsWithTag("TeleportAnchor");
        //foreach (GameObject obj in tutorialTeleportationAnchors)
        //{
            //obj.GetComponent<TeleportationAnchor>().teleportationProvider = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<TeleportationProvider>();
            //obj.GetComponent<TeleportationAnchor>().interactionManager = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<XRInteractionManager>();
        //}
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

    public void NextPatientPlayerSurgeryStage ()
    {
        if (playerMaterialID == (playerMaterials1.Length - 1))
        {
            playerMaterialID = 0;
        }
        else
        {
            playerMaterialID++;
        }
        
        GameObject.FindGameObjectWithTag("Body").GetComponent<SkinnedMeshRenderer>().material = playerMaterials1[playerMaterialID];
    }

    public void BackPatientPlayerSurgeryStage ()
    {
        if (playerMaterialID == 0)
        {
            playerMaterialID = (playerMaterials1.Length - 1);
        }
        else
        {
            playerMaterialID--;
        }
        
        GameObject.FindGameObjectWithTag("Body").GetComponent<SkinnedMeshRenderer>().material = playerMaterials1[playerMaterialID];
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
        GameObject spawnedPlayer = GameObject.FindGameObjectWithTag("Player");

        // TODO: fix players not spawning in world-specific locations
        if (id == 0)
        {
            //
            //spawnedPlayer.transform.position = spawnLocationMenu.position;
            //spawnedPlayer.transform.rotation = spawnLocationMenu.rotation;
        }
        else if (id == 1)
        {
            //GameObject spawnedPlayer = GameObject.FindGameObjectWithTag("Player");
            spawnedPlayer.transform.position = spawnLocationHospital.position;
            spawnedPlayer.transform.rotation = spawnLocationHospital.rotation;
        }
        else if (id == 2)
        {
            //GameObject spawnedPlayer = GameObject.FindGameObjectWithTag("Player");
            spawnedPlayer.transform.position = spawnLocationLivingroom.position;
            spawnedPlayer.transform.rotation = spawnLocationLivingroom.rotation;
        }
        else
        {
            //GameObject spawnedPlayer = GameObject.FindGameObjectWithTag("Player");
            spawnedPlayer.transform.position = spawnLocationMenu.position;
            spawnedPlayer.transform.rotation = spawnLocationMenu.rotation;
        }
    }

    public void ChangePlayerPosition (Transform t)
    {
        ScreenFadeOut();
        GameObject spawnedPlayer = GameObject.FindGameObjectWithTag("Player");
        spawnedPlayer.transform.position = t.position;
        spawnedPlayer.transform.rotation = t.rotation;
        ScreenFadeIn();
    }
}
