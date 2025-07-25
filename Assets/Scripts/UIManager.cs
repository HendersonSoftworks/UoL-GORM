using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Setup - Loaded on start")]
    [SerializeField]
    private Slider healthSlider;
    [SerializeField]
    private Slider staminaSlider;
    [SerializeField]
    private Slider manaSlider;
    [SerializeField]
    private PlayerCharacter playerCharacter;
    
    void Start()
    {
        playerCharacter = FindFirstObjectByType<PlayerCharacter>();
        healthSlider = GameObject.FindGameObjectWithTag("hSlider").GetComponent<Slider>();
        staminaSlider = GameObject.FindGameObjectWithTag("sSlider").GetComponent<Slider>();
        manaSlider = GameObject.FindGameObjectWithTag("mSlider").GetComponent<Slider>();
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
