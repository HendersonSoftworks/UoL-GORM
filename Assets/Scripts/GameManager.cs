using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PlayerCharacter playerCharacter;

    [SerializeField]
    private UIManager uiManager;

    public bool isGamePaused = false;
    //public int currentFloor = 0;

    private void Awake()
    {
        playerCharacter = FindFirstObjectByType<PlayerCharacter>();
        uiManager = GetComponent<UIManager>();
    }

    private void Start()
    {
        InitialiseGame();
    }
    
    private void InitialiseGame()
    {
        IncrementPlayerFloor(playerCharacter);
        SetFloorText(playerCharacter.currentFloor);
        uiManager.SlowlyDecreasePanelAlpha();
    }

    public void IncrementPlayerFloor(PlayerCharacter _playerCharacter)
    {
        _playerCharacter.currentFloor += 1;
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

    public void CloseContinueChoice()
    {
        uiManager.stairsContinuePanel.SetActive(false);
        SetPauseGame(false);
    }

    public void MoveToNextFloor()
    {
        IncrementPlayerFloor(playerCharacter);
        UnityEngine.SceneManagement.SceneManager.LoadScene("dungeon");
    }

    public void SetPauseGame(bool value)
    {
        isGamePaused = value;
        if (value) { Time.timeScale = 0; }
        else { Time.timeScale = 1; }
    }
}
