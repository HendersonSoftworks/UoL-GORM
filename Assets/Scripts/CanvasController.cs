using UnityEngine;

public class CanvasController : MonoBehaviour
{
    public MainMenuController menuController;

    public void StartGame()
    {
        menuController.LoadDungeon();

        GetComponent<Canvas>().worldCamera = FindFirstObjectByType<Camera>();
    }

    public void SetTitleScreenBool()
    {
        menuController.SetInTitleScreen();
    }

}
