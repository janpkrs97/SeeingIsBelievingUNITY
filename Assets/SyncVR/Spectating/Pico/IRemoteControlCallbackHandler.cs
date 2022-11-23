using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRemoteControlCallbackHandler
{
    void TakeRemoteControl();
    void ReleaseRemoteControl();
    void OptionSelected(int option_id);
}
