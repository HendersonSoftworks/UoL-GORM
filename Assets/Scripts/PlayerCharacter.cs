using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Character
{
    public enum Classes 
    {   
        None,
        warrior,
        Reaver,
        Berserker,
        Mage,
        Acolyte,
        Trader
    }
    public enum Races
    {
        None,
        Mestricarian,
        SaHranii,
        YuTai,
        GormiteZealot,
        Traealver,
        Morkalver,
        Dvaerg,
        Qothite
    }
    public enum SpecialAbilities
    {
        None,
        HolyDevotion,
        BetterBarter,
        DanceOfDeath,
        DramaticDeath,
        SummonBeasts,
        ShadowSyphon,
        HeavyHitter,
    }

    [Header("Player Config")]
    public Classes playerClass;
    public Races playerRace;
    public SpecialAbilities specialAbility;
    public float staminaRegainTimerReset = 1f;
    public float staminaRegainTimer = 1f;
    public bool inContactWithChest = false;
    public GameObject currentChest;

    [Header("Loaded on start")]
    private PlayerMovementController movementController;
    [SerializeField]
    private GameManager gameManager;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        DestroyDuplicates();

        gameManager = FindFirstObjectByType<GameManager>();
        movementController = GetComponent<PlayerMovementController>();
    }

    private void Start()
    {
        // Test
        SetTestStatblock();

        currentWeapon = weapons[0];

        CalculateMaxCurrentStats();
        SetMaxCurrentStats();

        staminaRegainTimer = staminaRegainTimerReset;
    }

    private void Update()
    {
        ClampStats();
        RegainStamina();
    }

    private void DestroyDuplicates()
    {
        var playerChars = FindObjectsByType<PlayerCharacter>(FindObjectsSortMode.None);
        if (playerChars.Length > 1) 
        {
            Destroy(gameObject);
        }
    }

    private void ClampStats()
    {
        strength      = (uint)Mathf.Clamp(strength, 0, 99f);
        dexterity     = (uint)Mathf.Clamp(dexterity, 0, 99f);
        constitution  = (uint)Mathf.Clamp(constitution, 0, 99f);
        intelligence  = (uint)Mathf.Clamp(intelligence, 0, 99f);
        wisdom        = (uint)Mathf.Clamp(wisdom, 0, 99f);
        charisma      = (uint)Mathf.Clamp(charisma, 0, 99f);
    }

    private void RegainStamina()
    {
        if (currentStamina == maxStamina) { return; }

        staminaRegainTimer -= Time.deltaTime;
        if (staminaRegainTimer > 0) { return; }
        staminaRegainTimer = staminaRegainTimerReset;

        if (currentStamina < maxStamina) { currentStamina += 1;}
    }

    public void SetTestStatblock()
    {
        strength += 5;
        dexterity += 5;
        constitution += 5;
        intelligence += 5;
        wisdom += 5;
        charisma += 5;
    }

    public override void CalculateMaxCurrentStats()
    {
        // HP
        maxHP += constitution;
        switch (playerClass)
        {
            case Classes.warrior:
                maxHP += 5;
                break;
            case Classes.Reaver:
                maxHP += 4;
                break;
            case Classes.Berserker:
                maxHP += 6;
                break;
            case Classes.Mage:
                maxHP += 3;
                break;
            case Classes.Acolyte:
                maxHP += 4;
                break;
            case Classes.Trader:
                maxHP += 4;
                break;
            default:
                break;
        }
        switch (playerRace)
        {
            case Races.Mestricarian:
                maxHP += 4;
                break;
            case Races.SaHranii:
                maxHP += 4;
                break;
            case Races.YuTai:
                maxHP += 5;
                break;
            case Races.GormiteZealot:
                maxHP += 3;
                break;
            case Races.Traealver:
                maxHP += 3;
                break;
            case Races.Morkalver:
                maxHP += 4;
                break;
            case Races.Dvaerg:
                maxHP += 6;
                break;
            case Races.Qothite:
                maxHP += 6;
                break;
            default:
                break;
        }

        // Stamina
        maxStamina += 1;
        maxStamina += strength;

        // Mana
        switch (playerRace)
        {
            case Races.None:
                break;
            case Races.Mestricarian:
                maxMana += 3;
                break;
            case Races.SaHranii:
                maxMana += 6;
                break;
            case Races.YuTai:
                maxMana += 3;
                break;
            case Races.GormiteZealot:
                maxMana += 4;
                break;
            case Races.Traealver:
                maxMana += 4;
                break;
            case Races.Morkalver:
                maxMana += 4;
                break;
            case Races.Dvaerg:
                maxMana += 5;
                break;
            case Races.Qothite:
                maxMana += 6;
                break;
            default:
                break;
        }
        switch (playerClass)
        {
            case Classes.Mage:
                maxMana += 6;
                break;
            case Classes.Acolyte:
                maxMana += 5;
                break;
            case Classes.Trader:
                maxMana += 4;
                break;
            default:
                break;
        }
    }

    private void SetMaxCurrentStats()
    {
        currentHP = (int)maxHP;
        currentStamina = maxStamina;
        currentMana = maxMana;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Damage player if hit by enemy attack
        if (collision.tag == "eHitbox")
        {
            movementController.PushPlayerInDirection(gameObject, collision.gameObject);
            DamageCharacter(collision.GetComponentInParent<EnemyCharacter>(), this);
        }

        // Assign chest to player
        if (collision.tag == "chest")
        {
            currentChest = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Unassign player chest 
        if (collision.tag == "chest")
        {
            currentChest = null;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Stairs")
        {
            gameManager.ShowContinueChoice(true);
        }
    }
}
