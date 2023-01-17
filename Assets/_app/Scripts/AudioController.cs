using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioController : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip buttonClick;

    public void OnButtonClick()
    {
        audioSource.clip = buttonClick;
        audioSource.Play();
    }
}
