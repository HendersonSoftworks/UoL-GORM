using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class UIManager : MonoBehaviour
{
    [Header("Setup - Loaded on start")]
    [SerializeField]
    private GameObject canvas; // added in gui
    [SerializeField]
    private Slider healthSlider;
    [SerializeField]
    private Slider staminaSlider;
    [SerializeField]
    private Slider manaSlider;
    [SerializeField]
    private PlayerCharacter playerCharacter;
    public GameObject stairsContinuePanel;
    public Button stairsYesButton;
    public TextMeshProUGUI floorText;
    public GameObject floorStartPanel; // added in gui
    public TextMeshProUGUI startFloorText; // added in gui
    public TextMeshProUGUI startFloorTipText; // added in gui

    private void Awake()
    {
        canvas.SetActive(true);
    }

    void Start()
    {
        playerCharacter = FindFirstObjectByType<PlayerCharacter>();
        healthSlider = GameObject.FindGameObjectWithTag("hSlider").GetComponent<Slider>();
        staminaSlider = GameObject.FindGameObjectWithTag("sSlider").GetComponent<Slider>();
        manaSlider = GameObject.FindGameObjectWithTag("mSlider").GetComponent<Slider>();
        stairsContinuePanel = GameObject.FindGameObjectWithTag("stairsContinuePanel");
        stairsYesButton = GameObject.FindGameObjectWithTag("stairsYesButton").GetComponent<Button>();
        floorText = GameObject.FindGameObjectWithTag("floorText").GetComponent<TextMeshProUGUI>();

        stairsContinuePanel.SetActive(false);
        floorStartPanel.SetActive(true);
    }

    void Update()
    {
        SetHPValues();
        SetStaminaValues();
        SetManaValues();
    }

    public void SlowlyDecreasePanelAlpha()
    {
        InvokeRepeating("DecreasePanelAlphaTransition", 4, 0.01f);
    }

    public void DecreasePanelAlphaTransition()
    {
        Image panelImage = floorStartPanel.GetComponent<Image>();
        var textObjs = panelImage.gameObject.GetComponentsInChildren<TextMeshProUGUI>();
        foreach (var item in textObjs)
        {
            item.gameObject.SetActive(false);
        }

        if (panelImage.color.a <= 0) 
        { 
            CancelInvoke("DecreasePanelAlphaTransition");
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
}
