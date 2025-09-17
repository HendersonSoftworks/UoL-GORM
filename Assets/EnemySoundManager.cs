using UnityEngine;

public class EnemySoundManager : MonoBehaviour
{
    [Header("Loaded on start")]
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip attackClip;
    [SerializeField]
    private AudioClip hurtClip;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayEnemyAttackClip()
    {
        audioSource.PlayOneShot(attackClip, AudioGlobalConfig.volEffects * AudioGlobalConfig.volScale);
    }

    public void PlayEnemyHurtClip()
    {
        audioSource.PlayOneShot(hurtClip, AudioGlobalConfig.volEffects * AudioGlobalConfig.volScale);
    }
}
