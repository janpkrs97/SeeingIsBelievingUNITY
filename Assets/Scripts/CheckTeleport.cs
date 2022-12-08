using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CheckTeleport : MonoBehaviour
{
    public PlayerController playerController;

    void Awake()
    {
        playerController = GameObject.Find("Managers").GetComponentInChildren<PlayerController>();
    }

    public void Teleporting()
    {
        playerController.ScreenFadeOutIn();
    }
}
