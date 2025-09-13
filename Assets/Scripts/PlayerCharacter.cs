using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

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
    [SerializeField]
    private PlayerMovementController movementController;
    [SerializeField]
    private GameManager gameManager;
    [SerializeField]
    private Animator animator;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        DestroyDuplicates();

        gameManager = FindFirstObjectByType<GameManager>();
        movementController = GetComponent<PlayerMovementController>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        // Test
        SetTestStatblock();

        currentWeapon = weapons[0];

        CalculateMaxCurrentStats();
        SetMaxCurrentStats();

        staminaRegainTimer = staminaRegainTimerReset;

        SetCharacterMods();
    }

    private void Update()
    {
        ClampStats();
        RegainStamina();
    }

    public void SetStarterSpell()
    {
        var starterSpell = spells[0];
        spells[0] = null;

        AddItem(starterSpell);
    }

    public void AddItem(Item _item)
    {
        if (_item is Ring)
        {
            Ring _ring = (Ring)_item;
            rings.Add(_ring);
            AddRingEffect(_ring.ringEffect, _ring);
        }
        else if (_item is Spell)
        {
            if (spells[0] == null)
            {
                spells[0] = (Spell)_item;
            }
            else if (spells[1] == null)
            {
                spells[1] = (Spell)_item;
            }
            else if (spells[2] == null)
            {
                spells[2] = (Spell)_item;
            }

            gameManager.UpdateSpellsHotBar(spells);
        }
    }

    public void AddRingEffect(Ring.ringEffects _ringEffect, Ring _ring)
    {
        switch (_ringEffect)
        {
            case Ring.ringEffects.none:

                break;
            case Ring.ringEffects.ac:
                armourClass += (uint)_ring.bonus;

                break;
            case Ring.ringEffects.damage:
                miscDamageBonus += (uint)_ring.bonus;

                break;
            case Ring.ringEffects.moveSpeed:
                movementController.moveSpeed += (uint)_ring.bonus * 10;
                movementController.baseSpeed += (uint)_ring.bonus * 10;

                break;
            case Ring.ringEffects.attackSpeed:
                float _attackSpeedMult = animator.GetFloat("attackSpeedMult");
                _attackSpeedMult += _ring.bonus;
                animator.SetFloat("attackSpeedMult", _attackSpeedMult);

                break;
            case Ring.ringEffects.castSpeed:
                float _castSpeedMult = animator.GetFloat("castSpeedMult");
                _castSpeedMult += _ring.bonus;
                animator.SetFloat("attackSpeedMult", _castSpeedMult);

                break;
            case Ring.ringEffects.torchLight:
                Light2D _light = GetComponentInChildren<Light2D>();
                _light.pointLightOuterRadius += _ring.bonus;

                break;
            default:

                break;
        }
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
