using System;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuAudio : MonoBehaviour
{
    [SerializeField]
    private AudioSource[] audioSources;
    [SerializeField]
    private AudioClip menuConfirmClip;
    [SerializeField]
    private AudioClip scaryLaughClip;
    [SerializeField]
    private Slider musicSlider;
    [SerializeField]
    private Slider effectsSlider;
    [SerializeField]
    private AudioClip effectsPreview;

    private void Start()
    {
        //Adds a listener to the main slider and invokes a method when the value changes.
        musicSlider.onValueChanged.AddListener(delegate { SetMusicAudioVol(); });
        effectsSlider.onValueChanged.AddListener(delegate { SetEffectsAudioVol(); });
    }

    private void Update()
    {
        audioSources[0].volume = AudioGlobalConfig.volMusic; // bug - cannot hear effects preview if music vol is 0
    }

    public void PlayMenuConfirmOneshot()
    {
        audioSources[1].PlayOneShot(menuConfirmClip, AudioGlobalConfig.volEffects * AudioGlobalConfig.volScale);        
    }

    public void PlayScaryLaughOneshot()
    {
        audioSources[1].PlayOneShot(scaryLaughClip, AudioGlobalConfig.volEffects * AudioGlobalConfig.volScale);
    }

    public void PlayEffectsPreviewOneshot()
    {
        audioSources[1].PlayOneShot(effectsPreview, AudioGlobalConfig.volEffects * AudioGlobalConfig.volScale);
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
