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
            SceneManager.LoadScene("MenuOLD");
        }
        else if (id == 1)
        {
            SceneManager.LoadScene("HospitalOLD");
        }
        else
        {
            SceneManager.LoadScene("Menu");
        }
        
        sceneID = id;
        playerController.SceneChanged(id);
    }
}
