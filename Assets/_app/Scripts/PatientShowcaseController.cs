using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles the functionality of improving visual feedback during the character selection UI.
public class PatientShowcaseController : MonoBehaviour
{
    [Header("GameObjects")]
    [Tooltip("The platforms characters are standing on.")]
    public GameObject[] showcasePlatforms;

    [Tooltip("The spot lights characters are standing underneath.")]
    public GameObject[] spotLights;

    [Header("Materials")]
    [Tooltip("The material used for all inactive character platforms.")]
    public Material defaultMat; 

    [Tooltip("The material used for the active character platform.")]
    public Material selectedMat;

    public void PatientSelected (int id)
    {
        // Check which character is currently selected. 0 - User selected the back button. Otherwise, change the material of the selected character's platform and activate its spotlight.
        if (id == 0)
        {
            // Reset all character platforms to their default material.
            foreach (GameObject sP in showcasePlatforms)
            {
                sP.GetComponent<Renderer>().material = defaultMat; 

                // Deactivate all character spot lights.
                foreach (GameObject sL in spotLights)
                {
                    sL.SetActive(false); 
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