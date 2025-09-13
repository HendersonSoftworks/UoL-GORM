using UnityEngine;
using UnityEngine.SceneManagement;

public class IntermediateSceneManager : MonoBehaviour
{
    void Start()
    {
        SceneManager.LoadScene("main_menu");
    }
}
