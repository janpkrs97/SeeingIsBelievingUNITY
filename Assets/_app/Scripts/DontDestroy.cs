using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Used by the manager scripts to ensure they are carried over when a new scene loads.
public class DontDestroy : MonoBehaviour 
{
    void Awake () 
    {
        GameObject.DontDestroyOnLoad(this.gameObject);
    }
}