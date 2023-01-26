using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles the main menu's functionality for resizing a character's height and arm length.
public class CalibratePlayerSize : MonoBehaviour
{
    [Header("Transforms")]
    [Tooltip("The left upper arm transform target of character.")]
    public Transform upperArmBoneLeft;
    
    [Tooltip("The left lower arm transform target of character.")]
    public Transform lowerArmBoneLeft;
    
    [Tooltip("The right upper arm transform target of character.")]
    public Transform upperArmBoneRight;
    
    [Tooltip("The right lower arm transform target of character.")]
    public Transform lowerArmBoneRight;
    
    [Header("Floats")]
    [Tooltip("The float value the transform targets are scaled by.")]
    public float scalePct = 0.015f;
    
    private float _scaleHeight, _scaleArms; // Used for easier script readability.

    // Increases the height of the character by scalePct.
    public void GrowHeight () 
    { 
        _scaleHeight = this.transform.localScale.y + scalePct;
        this.gameObject.transform.localScale = new Vector3(_scaleHeight, _scaleHeight, _scaleHeight);
    }

    // Decreases the height of the character by scalePct.
    public void ShrinkHeight () 
    { 
        _scaleHeight = this.transform.localScale.y - scalePct;
        this.gameObject.transform.localScale = new Vector3(_scaleHeight, _scaleHeight, _scaleHeight);
    }

    // Increases the length of the character's arms by scalePct.
    public void GrowArms () 
    { 
        _scaleArms = lowerArmBoneLeft.localScale.y + scalePct;
        lowerArmBoneLeft.localScale = upperArmBoneLeft.localScale = lowerArmBoneRight.localScale = upperArmBoneRight.localScale = new Vector3(_scaleArms, _scaleArms, _scaleArms);
    }

    // Decreases the length of the character's arms by scalePct.
    public void ShrinkArms () 
    { 
        _scaleArms = lowerArmBoneLeft.localScale.y - scalePct;
        lowerArmBoneLeft.localScale = upperArmBoneLeft.localScale = lowerArmBoneRight.localScale = upperArmBoneRight.localScale = new Vector3(_scaleArms, _scaleArms, _scaleArms);
    }
    
    // Resets character height and arm scale to the original transforms - needed when users go back and (re)select a different character.
    public void ResetPlayerSize () 
    { 
        this.gameObject.transform.localScale = new Vector3(1, 1, 1);
        lowerArmBoneLeft.localScale = upperArmBoneLeft.localScale = lowerArmBoneRight.localScale = upperArmBoneRight.localScale = new Vector3(1, 1, 1);
    }
}