using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MannequinController : MonoBehaviour
{
    public GameObject standingPatientMannequin;
    public GameObject lyingPatientMannequin;

    public Material[] mannequinMaterials;

    public void ChangeMannequinSurgeryStage (int id)
    {
        GameObject[] bodies = GameObject.FindGameObjectsWithTag("MannequinBody");

        foreach (GameObject obj in bodies)
        {
            obj.GetComponent<SkinnedMeshRenderer>().material = mannequinMaterials[id];
        }
        
        //bodies[0].GetComponent<SkinnedMeshRenderer>().material = mannequinMaterials[id];
        //bodies[1].GetComponent<SkinnedMeshRenderer>().material = mannequinMaterials[id];
        //standingPatientMannequin.GetComponent<SkinnedMeshRenderer>().material = mannequinMaterials[id];
        //lyingPatientMannequin.GetComponent<SkinnedMeshRenderer>().material = mannequinMaterials[id];
    }
}
