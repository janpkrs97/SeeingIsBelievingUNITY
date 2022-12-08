using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
