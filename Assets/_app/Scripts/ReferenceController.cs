using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// Handles the functionalities required when trying to access the PlayerController or SceneController objects via UI buttons from the Hospital and Home scenes.
public class ReferenceController : MonoBehaviour
{
    [Header("GameObjects")]
    [Tooltip("The <PlayerController> script under the <Player Manager> object.")]
    public PlayerController playerController;

    [Header("Transforms")]
    [Tooltip("The player's default position.")]
    public Transform playerPositionMain;

    [Tooltip("The player's position for the mirror scenario.")]
    public Transform playerPositionMirror;

    [Header("UI Elements")]
    [Tooltip("The mirror scenario's menu text which displays the active surgery stage.")]
    public TMP_Text mirrorSurgeryStageTxt;

    private GameObject _managersParent; // Reference to the external Managers parent object.
    private SceneController _sceneController; // Reference to the external SceneController script.

    void Awake()
    {
        // Find the external scripts and populate the appropiate variables.
        _managersParent = GameObject.FindGameObjectWithTag("Manager");
        playerController = _managersParent.GetComponentInChildren<PlayerController>();
        _sceneController = _managersParent.GetComponentInChildren<SceneController>();
    }

    // Calls the external function of the PlayerController script to change the player's active material to the next surgery stage.
    public void NextPlayerSurgeryStage ()
    {
        playerController.NextPatientPlayerSurgeryStage();
        mirrorSurgeryStageTxt.text = "SURGERY STAGE: " + playerController.playerMaterialID + "";
    }

    // Calls the external function of the PlayerController script to change the player's active material to the previous surgery stage.
    public void BackPlayerSurgeryStage ()
    {
        playerController.BackPatientPlayerSurgeryStage();
        mirrorSurgeryStageTxt.text = "SURGERY STAGE: " + playerController.playerMaterialID + "";
    }

    // Calls the external function of the PlayerController script to move the player to their default position.
    public void ChangePlayerPositionForMain ()
    {
        playerController.ChangePlayerPosition(playerPositionMain);
    }

    // Calls the external function of the PlayerController script to move the player to the mirror scenario position.
    public void ChangePlayerPositionForMirror ()
    {
        playerController.ChangePlayerPosition(playerPositionMirror);
    }

    // Calls the external function of the SceneController script to change to the main menu scene.
    public void ExitToMenu ()
    {
        _sceneController.ChangeScene(0);
    }
}
