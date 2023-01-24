using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MannequinController : MonoBehaviour
{
    public ReferenceController referenceController;
    public PlayerController playerController;
    public Material[] mannequinMaterialsR;
    public Material[] mannequinMaterialsS;
    public int materialID = 0;

    public TMP_Text surgeryStageTxt;

    public void Start ()
    {
        playerController = referenceController.playerController;

        if (playerController.playerMaterialStyle == 0) // Style: Realistic
        {
            GameObject.FindGameObjectWithTag("MannequinBody").GetComponent<SkinnedMeshRenderer>().material = mannequinMaterialsR[materialID];
        }
        else // Style: Stylized
        {
            GameObject.FindGameObjectWithTag("MannequinBody").GetComponent<SkinnedMeshRenderer>().material = mannequinMaterialsS[materialID];
        }    
    }

    public void ChangeMannequinSurgeryStage (int id)
    {
        GameObject[] bodies = GameObject.FindGameObjectsWithTag("MannequinBody");

        foreach (GameObject obj in bodies)
        {
            obj.GetComponent<SkinnedMeshRenderer>().material = mannequinMaterialsR[id];
        }
    }

    public void NextMannequinSurgeryStage ()
    {
        GameObject body = GameObject.FindGameObjectWithTag("MannequinBody");

        if (playerController.playerMaterialStyle == 0) // Style: Realistic
        {
            if (materialID == (mannequinMaterialsR.Length - 1))
            {
                materialID = 0;
            }
            else
            {
                materialID++;
            }

            body.GetComponent<SkinnedMeshRenderer>().material = mannequinMaterialsR[materialID];
        }
        else // Style: Stylized
        {
            if (materialID == (mannequinMaterialsS.Length - 1))
            {
                materialID = 0;
            }
            else
            {
                materialID++;
            }
            
            body.GetComponent<SkinnedMeshRenderer>().material = mannequinMaterialsS[materialID];
        }
        
        surgeryStageTxt.text = "SURGERY STAGE: " + materialID + "";
    }

    public void BackMannequinSurgeryStage ()
    {
        GameObject body = GameObject.FindGameObjectWithTag("MannequinBody");

        if (playerController.playerMaterialStyle == 0) // Style: Realistic
        {
            if (materialID == 0)
            {
                materialID = (mannequinMaterialsR.Length - 1);
            }
            else
            {
                materialID--;
            }

            body.GetComponent<SkinnedMeshRenderer>().material = mannequinMaterialsR[materialID];
        }
        else // Style: Stylized
        {
            if (materialID == 0)
            {
                materialID = (mannequinMaterialsR.Length - 1);
            }
            else
            {
                materialID--;
            }
            
            body.GetComponent<SkinnedMeshRenderer>().material = mannequinMaterialsS[materialID];
        }
        
        surgeryStageTxt.text = "SURGERY STAGE: " + materialID + "";
    }

    /*public void BackMannequinSurgeryStage () OLDONE for reference
    {
        if (materialID == 0)
        {
            materialID = (mannequinMaterialsR.Length - 1);
        }
        else
        {
            materialID--;
        }

        surgeryStageTxt.text = "SURGERY STAGE: " + materialID + "";
        GameObject body = GameObject.FindGameObjectWithTag("MannequinBody");
        body.GetComponent<SkinnedMeshRenderer>().material = mannequinMaterialsR[materialID];
    }*/
}
