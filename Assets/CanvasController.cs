using UnityEngine;

public class CanvasController : MonoBehaviour
{
    public MainMenuController menuController;

    public void StartGame()
    {
        menuController.LoadDungeon();
    }

    public void SetTitleScreenBool()
    {
        menuController.SetInTitleScreen();
    }

}
