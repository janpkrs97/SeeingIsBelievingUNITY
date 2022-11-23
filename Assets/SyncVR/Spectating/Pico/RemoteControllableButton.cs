using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RemoteControllableButton : Button
{
    public bool isRemoteControllable = true;

    public Text buttonText;
    public Text buttonCategory;

    private bool _remoteInteractable = true;
    private bool _localInteractable = true;

    public bool remoteInteractable
    {
        get
        {
            return _remoteInteractable;
        }
        set
        {
            _remoteInteractable = value;
            base.interactable = _remoteInteractable && _localInteractable;
        }
    }

    public bool localInteractable
    {
        get
        {
            return _localInteractable;
        }
        set
        {
            _localInteractable = value;
            base.interactable = _remoteInteractable && _localInteractable;
        }
    }
}
