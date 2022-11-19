using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferenceController : MonoBehaviour
{
    private GameObject managersParent;
    public PlayerController playerController;
    public SceneController sceneController;

    void Awake()
    {
        managersParent = GameObject.Find("Managers");
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

    public void ExitToMenu ()
    {
        sceneController.ChangeScene(0);
    }
}
