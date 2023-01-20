using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalibratePlayerSize : MonoBehaviour
{
    public Transform upperArmBoneLeft, lowerArmBoneLeft;
    public Transform upperArmBoneRight, lowerArmBoneRight;
    public Transform originalUpperArmBoneLeft, originalLowerArmBoneLeft, originalUpperArmBoneRight, originalLowerArmBoneRight;
    public float scalePct = 0.015f;
    private float scaleHeight, scaleArms;

    public void Start() 
    {
        originalUpperArmBoneLeft = upperArmBoneLeft;
        originalLowerArmBoneLeft = lowerArmBoneLeft;
        originalUpperArmBoneRight = upperArmBoneRight;
        originalLowerArmBoneRight = lowerArmBoneRight;
    }

    public void GrowHeight ()
    {
        scaleHeight = this.transform.localScale.y + scalePct;
        this.gameObject.transform.localScale = new Vector3(scaleHeight, scaleHeight, scaleHeight);
    }

    public void ShrinkHeight ()
    {
        scaleHeight = this.transform.localScale.y - scalePct;
        this.gameObject.transform.localScale = new Vector3(scaleHeight, scaleHeight, scaleHeight);
    }

    public void GrowArms ()
    {
        scaleArms = lowerArmBoneLeft.localScale.y + scalePct;
        lowerArmBoneLeft.localScale = upperArmBoneLeft.localScale = lowerArmBoneRight.localScale = upperArmBoneRight.localScale = 
            new Vector3(scaleArms, scaleArms, scaleArms);
    }

    public void ShrinkArms ()
    {
        scaleArms = lowerArmBoneLeft.localScale.y - scalePct;
        lowerArmBoneLeft.localScale = upperArmBoneLeft.localScale = lowerArmBoneRight.localScale = upperArmBoneRight.localScale = 
            new Vector3(scaleArms, scaleArms, scaleArms);
    }

    public void ResetDuplicatedPlayer()
    {
        // reset player height
        this.gameObject.transform.localScale = new Vector3(1, 1, 1);

        // reset player arms
        lowerArmBoneLeft.localScale = upperArmBoneLeft.localScale = lowerArmBoneRight.localScale = upperArmBoneRight.localScale = 
            new Vector3(1, 1, 1);
    }
}
