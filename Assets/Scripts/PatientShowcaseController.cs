using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatientShowcaseController : MonoBehaviour
{
    public GameObject[] showcasePlatforms;
    public Material defaultMat, selectedMat;

    public void PatientSelected (int id)
    {
        if (id == 0)
        {
            foreach (GameObject obj in showcasePlatforms)
            {
                obj.GetComponent<Renderer>().material = defaultMat;
            }
        }
        else
        {
            showcasePlatforms[id - 1].GetComponent<Renderer>().material = selectedMat;
        }
    }
}

