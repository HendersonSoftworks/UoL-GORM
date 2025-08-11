using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{   
    [SerializeField]
    private UIManager uiManager;

    public bool isGamePaused = false;

    private void Start()
    {
        uiManager = GetComponent<UIManager>();

        InitialiseGame();
    }
    
    private void InitialiseGame()
    {
        SetFloorText(1);
        uiManager.SlowlyDecreasePanelAlpha();
    }

    public void SetFloorText(int floorNumber)
    {
        string floorText = "F" + floorNumber;
        uiManager.floorText.text = floorText;
        uiManager.startFloorText.text = floorText;
        uiManager.startFloorTipText.text = "The only time we can be brave is when we are afraid.";
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
