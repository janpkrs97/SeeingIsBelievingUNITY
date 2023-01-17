using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;

public class ReferenceController : MonoBehaviour
{
    private GameObject managersParent;
    public PlayerController playerController;
    public SceneController sceneController;

    public Transform playerPositionMain;
    public Transform playerPositionMirror;

    public TMP_Text mirrorSurgeryStageTxt;

    void Awake()
    {
        managersParent = GameObject.FindGameObjectWithTag("Manager");
        playerController = managersParent.GetComponentInChildren<PlayerController>();
        sceneController = managersParent.GetComponentInChildren<SceneController>();
    }

    public void ChangePlayerVisualization (int id)
    {
        if (id == 0)
        {
            playerController.VisualizeFullBody();
        }
        else if (id == 1)
        {
            playerController.VisualizeUpperBodyOnly();
        }
        else if (id == 2)
        {
            playerController.VisualizeFaceOnly();
        }
        else
        {
            playerController.VisualizeFullBody();
        }
    }

    public void NextPlayerSurgeryStage ()
    {
        playerController.NextPatientPlayerSurgeryStage();
        mirrorSurgeryStageTxt.text = "SURGERY STAGE: " + playerController.playerMaterialID + "";
    }

    public void BackPlayerSurgeryStage ()
    {
        playerController.BackPatientPlayerSurgeryStage();
        mirrorSurgeryStageTxt.text = "SURGERY STAGE: " + playerController.playerMaterialID + "";
    }

    public void ChangePlayerSurgeryStage (int id)
    {
        playerController.PatientPlayerSurgeryStageChange(id);
    }

    public void ChangePlayerPositionForMain ()
    {
        playerController.ChangePlayerPosition(playerPositionMain);
    }

    public void ChangePlayerPositionForMirror ()
    {
        playerController.ChangePlayerPosition(playerPositionMirror);
    }

    public void ExitToMenu ()
    {
        playerController.DestroyPlayers();
        sceneController.ChangeScene(0);
    }
}
