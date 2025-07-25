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

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        CalculateMaxCurrentStats();
        SetMaxCurrentStats();
    }

    private void SetMaxCurrentStats()
    {
        currentHP = maxHP;
        currentStamina = maxStamina;
        currentMana = maxMana;
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
        strength += 3; // Testing
        dexterity += 3; // Testing
        maxStamina += ((strength / 2) + (dexterity / 2));

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
}
