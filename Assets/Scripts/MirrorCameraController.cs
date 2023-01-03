using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorCameraController : MonoBehaviour
{
    public void IncreaseHeight()
    {
        float prevPosY = this.gameObject.transform.position.y;
        float newPosY = prevPosY + 0.05f;
        this.gameObject.transform.position = new Vector3 (this.gameObject.transform.position.x, newPosY, this.gameObject.transform.position.z);
    }

    public void DecreaseHeight()
    {
        float prevPosY = this.gameObject.transform.position.y;
        float newPosY = prevPosY - 0.05f;
        this.gameObject.transform.position = new Vector3 (this.gameObject.transform.position.x, newPosY, this.gameObject.transform.position.z);
    }

    public void Refocus()
    {
        float playerHeadPosY = GameObject.FindGameObjectWithTag("PlayerHead").transform.position.y;
        this.gameObject.transform.position = new Vector3 (this.gameObject.transform.position.x, playerHeadPosY, this.gameObject.transform.position.z);
    }
}
