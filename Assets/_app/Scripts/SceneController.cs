using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Handles all required functionalities when switching to a new scene.
public class SceneController : MonoBehaviour
{
    [Tooltip("The <PlayerController> script under the <Player Manager> object.")]
    public PlayerController playerController;

    private int _sceneID; // 0 = Menu | 1 = Hospital | 2 = Home

    // When a scene change is called, first move the player's position and then load the new scene.
    public void ChangeScene (int id) 
    {
        _sceneID = id;

        if (_sceneID == 0)
        {
            playerController.ChangePlayerPosition(playerController.spawnLocationMenu);
            Destroy(GameObject.FindGameObjectWithTag("Manager")); // Destroy the Manager game object to avoid there being a duplicate.
            playerController.DestroyPlayer(); // Destroy the player's character as a new one will be created at the start of the Menu scene.
            SceneManager.LoadScene("Menu");
        }
        else if (_sceneID == 1)
        {
            playerController.ChangePlayerPosition(playerController.spawnLocationHospital);
            SceneManager.LoadScene("Hospital");
        }
        else if (_sceneID == 2)
        {
            playerController.ChangePlayerPosition(playerController.spawnLocationHome);
            SceneManager.LoadScene("Home");
        }
    }
}