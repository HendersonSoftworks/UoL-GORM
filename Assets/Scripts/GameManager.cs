using UnityEngine;

public class GameManager : MonoBehaviour
{   
    [SerializeField]
    private UIManager uiManager;

    public bool isGamePaused = false;

    private void Start()
    {
        uiManager = GetComponent<UIManager>();
    }

    public void ShowContinueChoice(bool value)
    {
        uiManager.stairsContinuePanel.SetActive(value);
        uiManager.stairsYesButton.Select();

        SetPauseGame(value);
    }

    public void SetPauseGame(bool value)
    {
        isGamePaused = value;
        if (value) { Time.timeScale = 0; }
        else { Time.timeScale = 0; }
    }
}
