using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MannequinController : MonoBehaviour
{
    public Material[] mannequinMaterials;
    public int materialID = 0;

    public TMP_Text surgeryStageTxt;

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
        
        surgeryStageTxt.text = "SURGERY STAGE: " + materialID + "";
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

        surgeryStageTxt.text = "SURGERY STAGE: " + materialID + "";
        GameObject body = GameObject.FindGameObjectWithTag("MannequinBody");
        body.GetComponent<SkinnedMeshRenderer>().material = mannequinMaterials[materialID];
    }
}
