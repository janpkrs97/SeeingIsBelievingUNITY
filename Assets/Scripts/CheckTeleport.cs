using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CheckTeleport : MonoBehaviour
{
    public PlayerController playerController;

    void Awake()
    {
        playerController = GameObject.FindGameObjectWithTag("Manager").GetComponentInChildren<PlayerController>();
    }

    public void Teleporting()
    {
        // if-statement for a/b testing patients 3/4 have screen fade, patients 0/1/2 do not -- remove to add screen fade for all players when teleporting
        if (playerController.playerID >= 2)
        {
            playerController.ScreenFadeOutIn();
        }
    }
}
