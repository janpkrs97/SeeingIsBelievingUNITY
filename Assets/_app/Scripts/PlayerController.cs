using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles all the functionalities relevant to the XR player.
public class PlayerController : MonoBehaviour
{
    [Header("GameObjects")]
    [Tooltip("The available character options.")]
    public GameObject[] xrPlayers;

    [Tooltip("The duplicated character used during the character size calibration process.")]
    public GameObject duplicatedPlayer;

    [Header("Transforms")]
    [Tooltip("The player's menu spawn location.")]
    public Transform spawnLocationMenu;

    [Tooltip("The player's hospital spawn location.")]
    public Transform spawnLocationHospital;

    [Tooltip("The player's home spawn location.")]
    public Transform spawnLocationHome;

    [Header("Materials")]
    [Tooltip("The realistic materials for character 1.")]
    public Material[] playerMaterials1R;
    
    [Tooltip("The stylized materials for character 1.")]
    public Material[] playerMaterials1S;

    [Header("Integers")]
    [Tooltip("The integer used to track the active surgery stage.")]
    public int playerMaterialID;
    
    [Header("Booleans")]
    [Tooltip("The boolean used to track the visualization style preference.")]
    public bool realisticMaterialStyle;

    private GameObject _spawnedPlayer; // Actively spawned character object.

    // Currently using single variables for skin meshes - this might need to change depending on MedicalVR's DL output.
    private SkinnedMeshRenderer _playerUpperBodySkinMesh;
    private SkinnedMeshRenderer _playerLowerBodySkinMesh;
    private SkinnedMeshRenderer _playerBody;
    private int _playerID; // Counter tracking the spawned character ID.

    void Start () 
    {
        // Spawn null character in main menu
        Instantiate(xrPlayers[0], spawnLocationMenu.position, spawnLocationMenu.rotation);
        _playerID = 0;
    }

    // Destroy all possible characters.
    public void DestroyPlayer ()
    {
        GameObject[] _spawnedPlayers = GameObject.FindGameObjectsWithTag("Player");
        
        foreach (GameObject gO in _spawnedPlayers)
        {
            Destroy(gO);
        }
    }

    // Spawn a new character and reset the duplicated character's size.
    public void SpawnAvatar (int id)
    {
        _spawnedPlayer = Instantiate(xrPlayers[id], spawnLocationMenu.position, spawnLocationMenu.rotation);
        _playerID = id;
        FindSkinMeshReferences();
        duplicatedPlayer.GetComponentInChildren<CalibratePlayerSize>().ResetPlayerSize();
        GameObject.DontDestroyOnLoad(_spawnedPlayer);
    }

    // Increase the (duplicated) character and its duplicate's height.
    public void IncreasePlayerHeight ()
    {
        _spawnedPlayer.GetComponentInChildren<CalibratePlayerSize>().GrowHeight();
        duplicatedPlayer.GetComponentInChildren<CalibratePlayerSize>().GrowHeight();
    }

    // Decrease the (duplicated) character and its duplicate's height.
    public void DecreasePlayerHeight ()
    {
        _spawnedPlayer.GetComponentInChildren<CalibratePlayerSize>().ShrinkHeight();
        duplicatedPlayer.GetComponentInChildren<CalibratePlayerSize>().ShrinkHeight();
    }

    // Increase the (duplicated) character and its duplicate's arm length.
    public void IncreasePlayerArms ()
    {
        _spawnedPlayer.GetComponentInChildren<CalibratePlayerSize>().GrowArms();
        duplicatedPlayer.GetComponentInChildren<CalibratePlayerSize>().GrowArms();
    }

    // Decrease the (duplicated) character and its duplicate's arm length.
    public void DecreasePlayerArms ()
    {
        _spawnedPlayer.GetComponentInChildren<CalibratePlayerSize>().ShrinkArms();
        duplicatedPlayer.GetComponentInChildren<CalibratePlayerSize>().ShrinkArms();
    }

    // Find the skin mesh references of the currently selected character. 
    public void FindSkinMeshReferences ()
    {
        _playerUpperBodySkinMesh = GameObject.FindGameObjectWithTag("PlayerUpperBody").GetComponent<SkinnedMeshRenderer>();
        _playerLowerBodySkinMesh = GameObject.FindGameObjectWithTag("PlayerLowerBody").GetComponent<SkinnedMeshRenderer>();
        _playerBody = GameObject.FindGameObjectWithTag("Body").GetComponent<SkinnedMeshRenderer>();
    }

    // Change the character's body visualization to full body.
    public void VisualizeFullBody ()
    {
        if (_playerUpperBodySkinMesh == null || _playerLowerBodySkinMesh == null)
        {
            FindSkinMeshReferences();
        }

        _playerUpperBodySkinMesh.enabled = true;
        _playerLowerBodySkinMesh.enabled = true;
    }

    // Change the character's body visualization to upper body only.
    public void VisualizeUpperBodyOnly ()
    {
        if (_playerUpperBodySkinMesh == null || _playerLowerBodySkinMesh == null)
        {
            FindSkinMeshReferences();
        }

        _playerUpperBodySkinMesh.enabled = true;
        _playerLowerBodySkinMesh.enabled = false;
    }

    // Change the character's body visualization to face only.
    public void VisualizeFaceOnly ()
    {
        if (_playerUpperBodySkinMesh == null || _playerLowerBodySkinMesh == null)
        {
            FindSkinMeshReferences();
        }

        _playerUpperBodySkinMesh.enabled = false;
        _playerLowerBodySkinMesh.enabled = false;
    }

    // Change the character's visualization style preference.
    public void AvatarStyle (int id)
    {
        if (id == 0)
        {
            realisticMaterialStyle = true;
            _playerBody.material = playerMaterials1R[playerMaterialID];
        }
        else
        {
            realisticMaterialStyle = false;
            _playerBody.material = playerMaterials1S[playerMaterialID];
        }
    }

    // Changes the character's active material to the next stage.
    public void NextPatientPlayerSurgeryStage ()
    {
        // Check the visualization style preference.
        if (realisticMaterialStyle)
        {
            // Check whether the active surgery stage is the last available stage. TRUE: Reset to first surgery stage. FALSE: Go to next surgery stage.
            if (playerMaterialID == (playerMaterials1R.Length - 1))
            {
                playerMaterialID = 0;
            }
            else
            {
                playerMaterialID++;
            }
            
            _playerBody.material = playerMaterials1R[playerMaterialID];
        }
        else
        {
            // Check whether the active surgery stage is the last available stage. TRUE: Reset to first surgery stage. FALSE: Go to next surgery stage.
            if (playerMaterialID == (playerMaterials1S.Length - 1))
            {
                playerMaterialID = 0;
            }
            else
            {
                playerMaterialID++;
            }
            
            _playerBody.material = playerMaterials1S[playerMaterialID];
        }
    }

    // Changes the character's active material to the previous stage.
    public void BackPatientPlayerSurgeryStage ()
    {
        // Check the visualization style preference.
        if (realisticMaterialStyle)
        {
            // Check whether the active surgery stage is the first available stage. TRUE: Go to last surgery stage. FALSE: Go to previous surgery stage.
            if (playerMaterialID == 0)
            {
                playerMaterialID = (playerMaterials1R.Length - 1);
            }
            else
            {
                playerMaterialID--;
            }
            
            _playerBody.material = playerMaterials1R[playerMaterialID];
        }
        else
        {
            // Check whether the active surgery stage is the first available stage. TRUE: Go to last surgery stage. FALSE: Go to previous surgery stage.
            if (playerMaterialID == 0)
            {
                playerMaterialID = (playerMaterials1S.Length - 1);
            }
            else
            {
                playerMaterialID--;
            }
            
            _playerBody.material = playerMaterials1S[playerMaterialID];
        }
    }

    // Creates a fade in effect by calling the SyncVRScreenFade component on the player camera.
    public void ScreenFadeIn ()
    {
        _spawnedPlayer.GetComponentInChildren<SyncVRScreenFade>().FadeIn();
    }

    // Creates a fade out effect by calling the SyncVRScreenFade component on the player camera.
    public void ScreenFadeOut ()
    {
        _spawnedPlayer.GetComponentInChildren<SyncVRScreenFade>().FadeOut();
    }

    // Handles physically moving the character for scene changes and (de)activation of the mirror scenario.
    public void ChangePlayerPosition (Transform t)
    {
        ScreenFadeOut();
        _spawnedPlayer.transform.position = t.position;
        _spawnedPlayer.transform.rotation = t.rotation;
        ScreenFadeIn();
    }
}