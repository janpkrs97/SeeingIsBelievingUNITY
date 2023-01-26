using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// Handles the functionality of changing the mannequin's body texture when switching between surgery stages, based on the visualization style preference.
public class MannequinController : MonoBehaviour
{
    [Header("Scripts")]
    [Tooltip("The <ReferenceController> script under the <Reference Manager> object.")]
    public ReferenceController referenceController;
    
    [Header("Materials")]
    [Tooltip("The realistic surgery stage materials.")]
    public Material[] mannequinMaterialsR;

    [Tooltip("The stylized surgery stage materials.")]
    public Material[] mannequinMaterialsS;

    [Header("UI Elements")]
    [Tooltip("The mannequin scenario's menu text which displays the active surgery stage.")]
    public TMP_Text surgeryStageTxt;

    private int _materialID = 0; // Used to track the active surgery stage.
    private PlayerController _playerController; // Reference to the external PlayerController script.
    private SkinnedMeshRenderer _body;

    public void Start ()
    {
        _playerController = referenceController.playerController;
        _body = GameObject.FindGameObjectWithTag("MannequinBody").GetComponent<SkinnedMeshRenderer>();

        // Initialize the mannequin's material according to the visualization style preference.
        if (_playerController.realisticMaterialStyle) 
        {
            _body.material = mannequinMaterialsR[_materialID];
        }
        else 
        {
            _body.material = mannequinMaterialsS[_materialID];
        }    
    }

    // Changes the mannequin's active material to the next stage.
    public void NextMannequinSurgeryStage ()
    {
        // Check the visualization style preference.
        if (_playerController.realisticMaterialStyle) 
        {
            // Check whether the active surgery stage is the last available stage. TRUE: Reset to first surgery stage. FALSE: Go to next surgery stage.
            if (_materialID == (mannequinMaterialsR.Length - 1))
            {
                _materialID = 0;
            }
            else 
            {
                _materialID++;
            }

            _body.GetComponent<SkinnedMeshRenderer>().material = mannequinMaterialsR[_materialID];
        }
        else
        {
            // Check whether the active surgery stage is the last available stage. TRUE: Reset to first surgery stage. FALSE: Go to next surgery stage.
            if (_materialID == (mannequinMaterialsS.Length - 1)) 
            {
                _materialID = 0;
            }
            else 
            {
                _materialID++;
            }
            
            _body.GetComponent<SkinnedMeshRenderer>().material = mannequinMaterialsS[_materialID];
        }
        
        surgeryStageTxt.text = "SURGERY STAGE: " + _materialID + "";
    }

    // Changes the mannequin's active material to the previous stage.
    public void BackMannequinSurgeryStage ()
    {
        // Check the visualization style preference.
        if (_playerController.realisticMaterialStyle)
        {
            // Check whether the active surgery stage is the first available stage. TRUE: Go to last surgery stage. FALSE: Go to previous surgery stage.
            if (_materialID == 0)
            {
                _materialID = (mannequinMaterialsR.Length - 1);
            }
            else
            {
                _materialID--;
            }

            _body.GetComponent<SkinnedMeshRenderer>().material = mannequinMaterialsR[_materialID]; 
        }
        else
        {
            // Check whether the active surgery stage is the first available stage. TRUE: Go to last surgery stage. FALSE: Go to previous surgery stage.
            if (_materialID == 0)
            {
                _materialID = (mannequinMaterialsR.Length - 1);
            }
            else
            {
                _materialID--;
            }
            
            _body.GetComponent<SkinnedMeshRenderer>().material = mannequinMaterialsS[_materialID];
        }
        
        surgeryStageTxt.text = "SURGERY STAGE: " + _materialID + "";
    }
}