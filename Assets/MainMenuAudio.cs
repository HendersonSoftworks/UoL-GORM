using System;
using UnityEngine;

public class MainMenuAudio : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip menuConfirmClip;
    [SerializeField]
    private AudioClip scaryLaughClip;

    public void PlayMenuConfirmOneshot()
    {
        audioSource.PlayOneShot(menuConfirmClip);        
    }

    internal void PlayScaryLaughOneshot()
    {
        audioSource.PlayOneShot(scaryLaughClip);
    }
}
