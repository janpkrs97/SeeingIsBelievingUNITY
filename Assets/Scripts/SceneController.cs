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
        if (id == 0)
        {
            playerController.ScreenFadeOut();
            SceneManager.LoadScene("Menu");
            playerController.ScreenFadeIn();
        }
        else if (id == 1)
        {
            playerController.ScreenFadeOut();
            SceneManager.LoadScene("HospitalNEW");
            playerController.ScreenFadeIn();
        }
        else if (id == 2)
        {
            playerController.ScreenFadeOut();
            SceneManager.LoadScene("Livingroom");
            playerController.ScreenFadeIn();
        }
        else
        {
            playerController.ScreenFadeOut();
            SceneManager.LoadScene("Menu");
            playerController.ScreenFadeIn();
        }
        
        sceneID = id;
        playerController.SceneChanged(id);
    }
}
