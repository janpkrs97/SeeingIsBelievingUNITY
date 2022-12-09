using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;
    public int sceneID;
    public PlayerController playerController;

    void Awake ()
    {
        // destroy 2nd manager object when menu scene is reloaded
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            DestroyImmediate(gameObject);
        }
    }

    public void ChangeScene (int id) 
    {
        playerController = GameObject.FindGameObjectWithTag("Manager").GetComponentInChildren<PlayerController>();
        playerController.ScreenFadeOut();

        if (id == 0)
        {
            SceneManager.LoadScene("Menu");
        }
        else if (id == 1)
        {
            SceneManager.LoadScene("HospitalNEW");
        }
        else if (id == 2)
        {
            SceneManager.LoadScene("Livingroom");
        }
        else
        {
            SceneManager.LoadScene("Menu");
        }
        
        sceneID = id;
        playerController.SceneChanged(sceneID);
        playerController.ScreenFadeIn();
    }
}
