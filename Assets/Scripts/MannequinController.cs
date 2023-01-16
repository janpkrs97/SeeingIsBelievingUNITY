using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MannequinController : MonoBehaviour
{
    public Material[] mannequinMaterials;
    public int materialID = 0;

    public void ChangeMannequinSurgeryStage (int id)
    {
        GameObject[] bodies = GameObject.FindGameObjectsWithTag("MannequinBody");

        foreach (GameObject obj in bodies)
        {
            obj.GetComponent<SkinnedMeshRenderer>().material = mannequinMaterials[id];
        }
    }

    public void NextMannequinSurgeryStage ()
    {
        if (materialID == (mannequinMaterials.Length - 1))
        {
            materialID = 0;
        }
        else
        {
            materialID++;
        }
        
        GameObject body = GameObject.FindGameObjectWithTag("MannequinBody");
        body.GetComponent<SkinnedMeshRenderer>().material = mannequinMaterials[materialID];
    }

    public void BackMannequinSurgeryStage ()
    {
        if (materialID == 0)
        {
            materialID = (mannequinMaterials.Length - 1);
        }
        else
        {
            materialID--;
        }

        GameObject body = GameObject.FindGameObjectWithTag("MannequinBody");
        body.GetComponent<SkinnedMeshRenderer>().material = mannequinMaterials[materialID];
    }
}
