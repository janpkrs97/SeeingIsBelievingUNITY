using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MannequinController : MonoBehaviour
{
    public Material[] mannequinMaterials;

    public void ChangeMannequinSurgeryStage (int id)
    {
        GameObject[] bodies = GameObject.FindGameObjectsWithTag("MannequinBody");

        foreach (GameObject obj in bodies)
        {
            obj.GetComponent<SkinnedMeshRenderer>().material = mannequinMaterials[id];
        }
    }
}
