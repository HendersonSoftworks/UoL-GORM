using UnityEngine;

public class ExplosionEventHelper : MonoBehaviour
{
    [SerializeField]
    private PlayerAudioManager playerAudio;

    private void Start()
    {
        playerAudio = FindFirstObjectByType<PlayerAudioManager>();

        if (playerAudio == null) { return; }

        playerAudio.PlayExplosionClip();
    }

    public void DestroyExplosion()
    {
        Destroy(gameObject);
    }
}
