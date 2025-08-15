using UnityEngine;

public class UIDontDestroyOnLoad : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        DestroyDuplicates();
    }

    private void DestroyDuplicates()
    {
        var objs = FindObjectsByType<UIDontDestroyOnLoad>(FindObjectsSortMode.None);
        if (objs.Length > 1)
        {
            Destroy(gameObject);
        }
    }
}
