using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class RemoteControlSceneManager : MonoBehaviour, IRemoteControlCallbackHandler
{
    private List<RemoteControllableButton> remoteControllableButtons = null;
    private List<RemoteControlManager.RemoteControlOption> remoteControlOptions = null;


    public virtual void Start()
    {
        if (RemoteControlManager.Instance != null)
        {
            RemoteControlManager.Instance.remoteControlCallbackHandler = this;

            if (RemoteControlManager.Instance.isRemoteControl)
            {
                TakeRemoteControl();
            }
        }
    }

    public virtual void OnDestroy()
    {
        RemoteControlManager.Instance.remoteControlCallbackHandler = null;
    }

    public virtual void TakeRemoteControl()
    {
        Debug.Log("OnTakeRemoteControl has also reached the RemoteControlSceneManager!");
        SendRemoteControlOptions();
        RemoteButtonsInteractable(false);
    }

    public virtual void ReleaseRemoteControl()
    {
        remoteControlOptions = null;
        RemoteButtonsInteractable(true);
    }

    public void OptionSelected(int option_id)
    {
        Debug.Log("Option Selected: " + option_id);
        remoteControllableButtons[option_id].onClick.Invoke();
    }

    /**
     * Button related functionality
     */
    protected void LocalButtonsInteractable(bool interactable)
    {
        FindObjectsOfType<RemoteControllableButton>().ToList().ForEach(x => x.localInteractable = interactable);
        FindObjectsOfType<Button>().ToList().ForEach(x => x.interactable = interactable);
    }

    protected void RemoteButtonsInteractable(bool interactable)
    {
        remoteControllableButtons.ForEach(x => x.remoteInteractable = interactable);
    }

    private List<RemoteControlManager.RemoteControlOption> CurrentRemoteControlOptions()
    {
        remoteControllableButtons = FindObjectsOfType<RemoteControllableButton>().ToList();

        remoteControlOptions = new List<RemoteControlManager.RemoteControlOption>();
        for (int i = 0; i < remoteControllableButtons.Count; i++)
        {
            RemoteControllableButton b = remoteControllableButtons[i];
            if (b.isRemoteControllable && b.IsActive() && b.localInteractable)
            {
                RemoteControlManager.RemoteControlOption option = new RemoteControlManager.RemoteControlOption();
                option.option_id = i;
                option.option_name = b.buttonText.text;
                option.option_category = b.buttonCategory == null ? "NO_CATEGORY" : b.buttonCategory.text;

                remoteControlOptions.Add(option);
            }
        }

        return remoteControlOptions;
    }

    public void SendRemoteControlOptions()
    {
        if (RemoteControlManager.Instance != null)
        {
            if (RemoteControlManager.Instance.isRemoteControl)
            {
                RemoteControlManager.Instance.SendRemoteControlOptions(CurrentRemoteControlOptions());
            }
        }
    }

}
