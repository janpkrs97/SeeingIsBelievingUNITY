using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferenceController : MonoBehaviour
{
    private GameObject managersParent;
    public PlayerController playerController;
    public SceneController sceneController;
    //public MannequinController mannequinController;

    void Awake()
    {
        managersParent = GameObject.FindGameObjectWithTag("Manager");
        playerController = managersParent.GetComponentInChildren<PlayerController>();
        sceneController = managersParent.GetComponentInChildren<SceneController>();
        //mannequinController = GameObject.Find("MannequinScenario").GetComponent<MannequinController>();
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

    public void ChangePlayerSurgeryStage (int id)
    {
        playerController.PatientPlayerSurgeryStageChange(id);
    }

    public void ExitToMenu ()
    {
        playerController.DestroyPlayers();
        sceneController.ChangeScene(0);
    }
}
