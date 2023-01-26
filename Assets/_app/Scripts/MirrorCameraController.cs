using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles the functionality of automatically adjusting the height of the mirror's camera to accomodate various user heights.
public class MirrorCameraController : MonoBehaviour
{
    public void Refocus ()
    {
        float _playerHeadPosY = GameObject.FindGameObjectWithTag("PlayerHead").transform.position.y;
        this.gameObject.transform.position = new Vector3 (this.gameObject.transform.position.x, _playerHeadPosY, this.gameObject.transform.position.z);
    }
}