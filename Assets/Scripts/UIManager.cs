using UnityEngine;
using UnityEngine.UI;

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

        stairsContinuePanel.SetActive(false);
    }

    void Update()
    {
        SetHPValues();
        SetStaminaValues();
        SetManaValues();
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
