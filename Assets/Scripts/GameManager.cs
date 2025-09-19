using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum floorTypes { dungeon, swamp, infernal, dream}

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

    private void FindMusicManager()
    {
        audioSource = FindFirstObjectByType<DungeonMusicManager>().audioSource;
    }

    private void SetFloorType()
    {
        if (Database.currentFloor <= 1)
        {
            floorType = floorTypes.dungeon;
        }
        else if (Database.currentFloor == 2)
        {
            floorType = floorTypes.swamp;
        }
        else if (Database.currentFloor == 3)
        {
            floorType = floorTypes.infernal;
        }
        else if (Database.currentFloor >= 4)
        {
            floorType = floorTypes.dream;
        }
        else
        {
            floorType = floorTypes.dream;
        }
        
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

        SetFloorType();

        ResetPlayerPos();

        isGamePaused = true;
        uiManager.SlowlyDecreasePanelAlpha();

        Invoke("FindMusicManager", 2);
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
        uiManager.startFloorTipText.text = Database.quotes[
            UnityEngine.Random.Range(0, Database.quotes.Length)];
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

        if (Database.currentFloor < 5)
        {
            SceneManager.LoadScene("dungeon");
        }
        else
        {
            SceneManager.LoadScene("boss_dungeon");
        }
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

    public void BossDeath()
    {
        StartCoroutine(TransitionToCreditScene());
    }

    public IEnumerator TransitionToCreditScene()
    {
        yield return new WaitForSeconds(0.75f);

        isGamePaused = true;
        
        uiManager.floorStartPanel.SetActive(true);
        uiManager.floorStartPanel.GetComponent<Image>().color = Color.black;
        uiManager.startFloorText.gameObject.SetActive(true);
        uiManager.startFloorTipText.gameObject.SetActive(true);
        uiManager.startFloorText.text = "A game by Thomas Henderson.";
        uiManager.startFloorTipText.text = "Thank you for playing.";
        uiManager.startFloorText.fontSize = 128;
        uiManager.startFloorTipText.fontSize = 128;

        StartCoroutine(DelayedLoadMainMenu());
    }

    public IEnumerator DelayedLoadMainMenu()
    {
        yield return new WaitForSeconds(1.25f);
        LoadMainMenu();
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

    public void ReloadGame()
    {
        foreach (var persistent in PersistentObjects)
        {
            Destroy(persistent);
        }

        Time.timeScale = 1;

        Database.currentFloor = 1;

        SceneManager.LoadScene("dungeon", LoadSceneMode.Single);
    }

    public void OpenStatsScreen()
    {
        uiManager.statsPanel.SetActive(true);

        var descString = "";

        foreach (var ring in playerCharacter.rings)
        {
            descString += ring.itemName + " - ";
            descString += ring.description;
            descString += "\n";
        }

        uiManager.statsDesc.text = descString;

        SetPauseGame(true);
    }

    public void CloseStatsScreen()
    {
        uiManager.statsPanel.SetActive(false);
        SetPauseGame(false);
    }

    public void OpenSpellScreen()
    {
        uiManager.spellsPanel.SetActive(true);

        var descString = "";

        foreach (var spell in playerCharacter.spells)
        {
            if (spell == null){ continue; }

            descString += spell.itemName + " - ";
            descString += spell.description + "\n";
            descString += "Power: " + spell.effectValue;
            descString += "\n";
        }

        uiManager.spellsDesc.text = descString;

        SetPauseGame(true);
    }

    public void CloseSpellScreen()
    {
        uiManager.spellsPanel.SetActive(false);
        SetPauseGame(false);
    }

    #endregion
}
