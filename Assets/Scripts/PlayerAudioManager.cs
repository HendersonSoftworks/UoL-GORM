using UnityEngine;

public class PlayerAudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioClip explClip;
    [SerializeField]
    private AudioClip swingClip;
    [SerializeField]
    private AudioClip hurtClip;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayExplosionClip()
    {
        audioSource.PlayOneShot(explClip,  AudioGlobalConfig.volEffects * AudioGlobalConfig.volScale);
    }

    public void PlaySwingClip()
    {
        audioSource.PlayOneShot(swingClip, AudioGlobalConfig.volEffects * AudioGlobalConfig.volScale);
    }

    public void PlayHurtClip()
    {
        if (audioSource.isPlaying) { return; }

        audioSource.PlayOneShot(hurtClip, AudioGlobalConfig.volEffects * AudioGlobalConfig.volScale);
    }
}
