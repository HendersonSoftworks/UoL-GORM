using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;
using System;

public class UIManager : MonoBehaviour
{
    [Header("Setup - Loaded on start")]
    [SerializeField]
    private GameObject canvas; // added in gui
    [SerializeField]
    private GameManager gameManager;
    private PlayerCharacter playerCharacter;
    public GameObject stairsContinuePanel; // added in gui
    public Button stairsYesButton; // added in gui
    public TextMeshProUGUI floorText;
    public GameObject floorStartPanel; // added in gui
    public TextMeshProUGUI startFloorText; // added in gui
    public TextMeshProUGUI startFloorTipText; // added in gui
    public GameObject chestPanel;
    public Button chestTakeButton;
    public Image chestItemImage;
    public TextMeshProUGUI chestItemName;
    public TextMeshProUGUI chestItemDescription;

    [Header("Setup - Pause Menus")]
    public GameObject PauseMenuPanel;
    public GameObject DeathPanel;

    [Header("Setup - Player")]
    [SerializeField]
    private Slider healthSlider;
    [SerializeField]
    private Slider staminaSlider;
    [SerializeField]
    private Slider manaSlider;
    
    public Image primarySpellImage;
    public Image secorndarySpellImage;
    public Image tertiarySpellImage;
    public Image[] spellImages;

    [Header("References - Stat Screens")]
    public GameObject statsPanel;
    public TextMeshProUGUI statsDesc;
    public GameObject spellsPanel;
    public TextMeshProUGUI spellsDesc;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        gameManager = FindFirstObjectByType<GameManager>();
        playerCharacter = FindFirstObjectByType<PlayerCharacter>();
        healthSlider = GameObject.FindGameObjectWithTag("hSlider").GetComponent<Slider>();
        staminaSlider = GameObject.FindGameObjectWithTag("sSlider").GetComponent<Slider>();
        manaSlider = GameObject.FindGameObjectWithTag("mSlider").GetComponent<Slider>();
        floorText = GameObject.FindGameObjectWithTag("floorText").GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        spellImages[0] = primarySpellImage;
        spellImages[1] = secorndarySpellImage;
        spellImages[2] = tertiarySpellImage;
    }

    void Update()
    {
        SetHPValues();
        SetStaminaValues();
        SetManaValues();
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
        gameManager.isGamePaused = true; // Soft pause, timescale unaffected
        InitialiseUI();
    }

    private void InitialiseUI()
    {
        canvas.SetActive(true);
        ResetPanelAlphaAndText();
        floorStartPanel.SetActive(true);
        stairsContinuePanel.SetActive(false);
    }

    public void SlowlyDecreasePanelAlpha()
    {
        InvokeRepeating("DecreasePanelAlphaTransitionAndUnpause", 4, 0.01f);
    }

    public void ResetPanelAlphaAndText()
    {
        Image panelImage = floorStartPanel.GetComponent<Image>();
        var textObjs = panelImage.gameObject.GetComponentsInChildren<TextMeshProUGUI>(true);
        foreach (var item in textObjs)
        {
            item.gameObject.SetActive(true);
        }

        panelImage.color = new Color(panelImage.color.r,
            panelImage.color.g,
            panelImage.color.b,
            1);
    }

    public void DecreasePanelAlphaTransitionAndUnpause()
    {
        Image panelImage = floorStartPanel.GetComponent<Image>();
        var textObjs = panelImage.gameObject.GetComponentsInChildren<TextMeshProUGUI>();
        foreach (var item in textObjs)
        {
            item.gameObject.SetActive(false);
        }

        // Unpause as soon as panel starts to fade
        if (panelImage.color.a < 1) { gameManager.isGamePaused = false; }

        if (panelImage.color.a <= 0) 
        {
            CancelInvoke("DecreasePanelAlphaTransitionAndUnpause");
            floorStartPanel.SetActive(false);
        }

        panelImage.color = new Color(panelImage.color.r,
            panelImage.color.g,
            panelImage.color.b,
            panelImage.color.a - 0.01f);
    }

    public void SetHPValues()
    {
        healthSlider.maxValue = playerCharacter.maxHP;
        healthSlider.value = playerCharacter.currentHP;
    }

    public void SetStaminaValues()
    {
        staminaSlider.maxValue = playerCharacter.maxStamina;
        staminaSlider.value = playerCharacter.currentStamina;
    }

    public void SetManaValues()
    {
        manaSlider.maxValue = playerCharacter.maxMana;
        manaSlider.value = playerCharacter.currentMana;
    }

    public void SetCurrentSpellSlotSelected(int _spellSlotNum)
    {
        for (int i = 0; i < spellImages.Length; i++)
        {
            spellImages[i].color = new Color(
                spellImages[i].color.r, 
                spellImages[i].color.g,
                spellImages[i].color.b,
                0.2f);
            if (i == _spellSlotNum)
            {
                spellImages[i].color = new Color(
                    spellImages[i].color.r,
                    spellImages[i].color.g,
                    spellImages[i].color.b,
                    1f);
            }
        }
    }
}
