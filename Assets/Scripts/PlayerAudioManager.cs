using UnityEngine;

public class PlayerAudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioClip explClip;
    [SerializeField]
    private AudioClip swingClip;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayExplosionClip()
    {
        audioSource.PlayOneShot(explClip, 3 * AudioGlobalConfig.volScale);
    }

    public void PlayswingClip()
    {
        audioSource.PlayOneShot(swingClip, 3 * AudioGlobalConfig.volScale);
    }
}
