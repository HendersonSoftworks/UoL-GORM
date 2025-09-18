using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum floorTypes { dungeon, swamp, infernal}

public class GameManager : MonoBehaviour
{
    [Header("Game Logic")]
    public bool isGamePaused = false;
    public int currentSessionFloor = 0;
    public GameObject[] PersistentObjects;
    public floorTypes floorType = floorTypes.dungeon;

    [Header("References - Loaded on startup")]
    public PlayerCharacter playerCharacter;
    public UIManager uiManager;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip deathMusicTrack;
    public AudioClip impertantDeathClip;

    #region Private Methods

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        DestroyDuplicates();
        
        playerCharacter = FindFirstObjectByType<PlayerCharacter>();
        uiManager = GetComponent<UIManager>();
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

    #endregion

    #region Public Methods

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

    public void UpdateSpellsHotBar(List<Spell> _spells)
    {
        if (_spells[0] != null) { SetSpellHotbarSprite(_spells, 0); }
        if (_spells[1] != null) { SetSpellHotbarSprite(_spells, 1); }
        if (_spells[2] != null) { SetSpellHotbarSprite(_spells, 2); }
    }

    public void SetSpellHotbarSprite(List<Spell> _spells, int _index)
    {
        uiManager.spellImages[_index].sprite = _spells[_index].sprite;
        uiManager.spellImages[_index].color = _spells[_index].GetComponent<SpriteRenderer>().color;
    }

    public void ShowChestChoice()
    {
        if (isGamePaused) { return; }

        uiManager.chestTakeButton.interactable = true;

        // Populate item UI
        uiManager.chestItemImage.sprite =
            playerCharacter.currentChest.GetComponent<Chest>().chestItem.sprite;
        uiManager.chestItemImage.color = playerCharacter.currentChest
            .GetComponent<Chest>().chestItem.GetComponent<SpriteRenderer>().color;

        uiManager.chestItemName.text =
            playerCharacter.currentChest.GetComponent<Chest>().chestItem.name;
        uiManager.chestItemDescription.text =
            playerCharacter.currentChest.GetComponent<Chest>().chestItem.description;

        uiManager.chestPanel.SetActive(true);
        uiManager.chestTakeButton.Select();
        
        SetPauseGame(true);

        // Disable Take button if spell already obtained
        if (playerCharacter.currentChest.GetComponent<Chest>().chestItem is Spell)
        {
            foreach (var spell in playerCharacter.spells)
            {
                if (spell != null)
                {
                    if (spell.itemName == playerCharacter.currentChest.GetComponent<Chest>().chestItem.itemName)
                    {
                        print(spell.itemName);
                        print(playerCharacter.currentChest.GetComponent<Chest>().chestItem.itemName);
                        uiManager.chestTakeButton.interactable = false;
                        return;
                    }
                }
            }
        }
    }

    public void TakeItem()
    {
        if (playerCharacter == null) { return; }
        Item _chestitem = playerCharacter.currentChest.GetComponent<Chest>().chestItem;
        if (_chestitem == null) { return; }

        playerCharacter.AddItem(_chestitem);

        uiManager.chestPanel.SetActive(false);
        SetPauseGame(false);
        playerCharacter.currentChest.GetComponent<Chest>().MarkChestAsEmpty();
        playerCharacter.currentChest = null;
        playerCharacter.inContactWithChest = false;
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

    public void OpenPauseMenu()
    {
        uiManager.PauseMenuPanel.SetActive(true);
        SetPauseGame(true);
    }

    public void ClosePauseMenu()
    {
        uiManager.PauseMenuPanel.SetActive(false);
        SetPauseGame(false);
    }

    public void PlayImportantDeathSound()
    {
        audioSource.Stop();

        Time.timeScale = 0.25f;
        
        audioSource.PlayOneShot(impertantDeathClip, AudioGlobalConfig.volEffects * AudioGlobalConfig.volScale);
    }

    public void StartDeathCoroutine()
    {
        StartCoroutine(DelayedOpenDeathPanel());
    }

    public IEnumerator DelayedOpenDeathPanel()
    {
        yield return new WaitForSeconds(0.75f);

        Time.timeScale = 1f;

        OpenDeathMenu();
    }

    public void OpenDeathMenu()
    {
        uiManager.DeathPanel.SetActive(true);
        SetPauseGame(true);

        audioSource.PlayOneShot(deathMusicTrack, AudioGlobalConfig.volEffects * AudioGlobalConfig.volScale);
    }

    public void LoadMainMenu()
    {
        foreach (var persistent in PersistentObjects)
        {
            Destroy(persistent);
        }

        SceneManager.LoadScene("main_menu", LoadSceneMode.Single);
    }

    public void LoadCampsite()
    {
        foreach (var persistent in PersistentObjects)
        {
            Destroy(persistent);
        }

        SceneManager.LoadScene("camp", LoadSceneMode.Single);
    }

    #endregion
}
