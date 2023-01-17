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
        // A/b testing of screen fade (avatars 2,4 do not fade, avatars 0,1,3 have fade on teleport) -- edit depending on test results
        if (playerController.playerID % 2 != 0 || playerController.playerID == 0)
        {
            playerController.ScreenFadeOutIn();
        }
    }
}
