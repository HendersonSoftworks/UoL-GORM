using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Game Logic")]
    public bool isGamePaused = false;
    public int currentSessionFloor = 0;

    [Header("References - Loaded on startup")]
    public PlayerCharacter playerCharacter;
    [SerializeField]
    private UIManager uiManager;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        DestroyDuplicates();
        playerCharacter = FindFirstObjectByType<PlayerCharacter>();
        uiManager = GetComponent<UIManager>();
    }

    private void Start()
    {
        //InitialiseGame();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        InitialiseGame();
    }

    private void DestroyDuplicates()
    {
        var objs = FindObjectsByType<GameManager>(FindObjectsSortMode.None);
        if (objs.Length > 1)
        {
            Destroy(gameObject);
        }
    }

    private void InitialiseGame()
    {
        SetDungeonFloor(Database.currentFloor);
        SetFloorText(currentSessionFloor);
        ResetPlayerPos();
        isGamePaused = true;
        uiManager.SlowlyDecreasePanelAlpha();
    }

    private void ResetPlayerPos()
    {
        playerCharacter.transform.position = Vector3.zero;
    }

    private void SetDungeonFloor(int _currentFloor)
    {
        currentSessionFloor = _currentFloor;
    }

    public void IncrementFloorDatabase()
    {
        Database.currentFloor += 1;
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

    public void ShowChestChoice()
    {
        if (isGamePaused) { return; }

        // Populate item UI
        uiManager.chestItemImage.sprite =
            playerCharacter.currentChest.GetComponent<Chest>().chestItem.image;
        uiManager.chestItemName.text =
            playerCharacter.currentChest.GetComponent<Chest>().chestItem.name;
        uiManager.chestItemDescription.text =
            playerCharacter.currentChest.GetComponent<Chest>().chestItem.description;

        uiManager.chestPanel.SetActive(true);
        uiManager.chestTakeButton.Select();
        
        SetPauseGame(true);
    }

    public void CloseChestChoice()
    {
        uiManager.chestPanel.SetActive(false);
        playerCharacter.currentChest.GetComponent<Chest>().SetDialogue(playerCharacter.GetComponent<Collider2D>(), false);

        SetPauseGame(false);
    }

    public void MoveToNextFloor()
    {
        IncrementFloorDatabase();
        SetPauseGame(false);
        SceneManager.LoadScene("dungeon");
    }

    public void SetPauseGame(bool value)
    {
        isGamePaused = value;
        if (value) { Time.timeScale = 0; }
        else { Time.timeScale = 1; }
    }
}
