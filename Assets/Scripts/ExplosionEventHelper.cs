using UnityEngine;

public class ExplosionEventHelper : MonoBehaviour
{
    private PlayerAudioManager playerAudio;

    private void Start()
    {
        playerAudio = FindFirstObjectByType<PlayerAudioManager>();
        playerAudio.PlayExplosionClip();
    }

    public void DestroyExplosion()
    {
        Destroy(gameObject);
    }
}
