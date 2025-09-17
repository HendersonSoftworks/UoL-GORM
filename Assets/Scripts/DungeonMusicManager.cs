using UnityEngine;

public class DungeonMusicManager : MonoBehaviour
{
    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        audioSource.volume = AudioGlobalConfig.volMusic;
    }
}
