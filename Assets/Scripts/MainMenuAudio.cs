using System;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuAudio : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip menuConfirmClip;
    [SerializeField]
    private AudioClip scaryLaughClip;
    [SerializeField]
    private Slider audioSlider;

    private void Start()
    {
        //Invoke("InvokedSetSlider", 2);
    }

    private void Update()
    {
        audioSource.volume = AudioGlobalConfig.volScale;
    }

    private void InvokedSetSlider()
    {
        audioSlider.value = AudioGlobalConfig.volScale;
    }

    public void PlayMenuConfirmOneshot()
    {
        audioSource.PlayOneShot(menuConfirmClip);        
    }

    public void PlayScaryLaughOneshot()
    {
        audioSource.PlayOneShot(scaryLaughClip);
    }

    public void SetGlobalAudioVol()
    {
        if (audioSlider == null)
        {
            audioSlider = FindFirstObjectByType<Slider>(FindObjectsInactive.Include);
            if (audioSlider == null) return; 
        }

        AudioGlobalConfig.volScale = audioSlider.value;
    }
}
