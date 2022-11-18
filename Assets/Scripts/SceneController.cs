using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public int sceneID;

    public PlayerController playerController;

    public void Awake ()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void ChangeScene (int id) 
    {
        sceneID = id;
        SceneManager.LoadScene("Hospital");
        playerController.SceneChanged(id);
    }
}
