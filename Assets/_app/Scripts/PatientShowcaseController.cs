using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatientShowcaseController : MonoBehaviour
{
    public GameObject[] showcasePlatforms;
    public GameObject[] spotLights;
    public Material defaultMat, selectedMat;

    public void PatientSelected (int id)
    {
        if (id == 0)
        {
            foreach (GameObject obj in showcasePlatforms)
            {
                obj.GetComponent<Renderer>().material = defaultMat;

                foreach (GameObject obje in spotLights)
                {
                    obje.SetActive(false);
                }
            }
        }
        else
        {
            showcasePlatforms[id - 1].GetComponent<Renderer>().material = selectedMat;
            spotLights[id - 1].SetActive(true);
        }
    }
}

