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

    [Header("Loaded on start")]
    private PlayerMovementController movementController;

    private void Start()
    {
        movementController = GetComponent<PlayerMovementController>();

        currentWeapon = weapons[0];

        DontDestroyOnLoad(gameObject);

        CalculateMaxCurrentStats();
        SetMaxCurrentStats();

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
        if (collision.tag == "eHitbox")
        {
            movementController.PushPlayerInDirection(gameObject, collision.gameObject);
            DamageCharacter(collision.GetComponentInParent<EnemyCharacter>(), this);
        }
    }
}
