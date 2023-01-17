using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public int sceneID;
    public PlayerController playerController;

    public void ChangeScene (int id) 
    {
        sceneID = id;
        playerController.SceneChanged(id);

        if (id == 0)
        {
            playerController.ScreenFadeOut();
            Destroy(GameObject.FindGameObjectWithTag("Manager"));
            SceneManager.LoadScene("Menu");
            playerController.ScreenFadeIn();
        }
        else if (id == 1)
        {
            playerController.ScreenFadeOut();
            SceneManager.LoadScene("Hospital");
            playerController.ScreenFadeIn();
        }
        else if (id == 2)
        {
            playerController.ScreenFadeOut();
            SceneManager.LoadScene("Home");
            playerController.ScreenFadeIn();
        }
    }
}
