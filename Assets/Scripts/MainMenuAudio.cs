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
    private Slider musicSlider;
    [SerializeField]
    private Slider effectsSlider;

    private void Start()
    {
        //Adds a listener to the main slider and invokes a method when the value changes.
        musicSlider.onValueChanged.AddListener(delegate { SetMusicAudioVol(); });
        effectsSlider.onValueChanged.AddListener(delegate { SetEffectsAudioVol(); });
    }

    private void Update()
    {
        audioSource.volume = AudioGlobalConfig.volMusic;

    }

    public void PlayMenuConfirmOneshot()
    {
        audioSource.PlayOneShot(menuConfirmClip);        
    }

    public void PlayScaryLaughOneshot()
    {
        audioSource.PlayOneShot(scaryLaughClip);
    }

    public void SetMusicAudioVol()
    {
        AudioGlobalConfig.volMusic = musicSlider.value;
    }

    public void SetEffectsAudioVol()
    {
        AudioGlobalConfig.volEffects = effectsSlider.value;
    }
}
