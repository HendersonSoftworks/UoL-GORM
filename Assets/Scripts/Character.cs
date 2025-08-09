using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    #region Character Sheet

    [Header("Identity")]
    public string[] names;
    public string displayName;
    public Sprite portrait;

    [Header("Max & Current Stats")]
    public uint maxHP;
    public uint currentHP;
    public float maxStamina;
    public float currentStamina;
    public uint maxMana;
    public uint currentMana;

    [Header("Modifiers")]
    public uint statDamageBonus;
    public uint miscDamageBonus;
    public uint armourClass;
    public uint miscarmourBonus;

    [Header("Statblock")]
    public uint strength;
    public uint dexterity;
    public uint constitution;
    public uint intelligence;
    public uint wisdom;
    public uint charisma;

    [Header("Gear")]
    public Weapon currentWeapon;
    public Weapon[] weapons = new Weapon[3];
    public Armour headArmour;
    public Armour bodyArmour;
    public Ring[] rings = new Ring[2];

    [Header("Spellcasting")]
    public List<Spell> spells;

    [Header("Items")]
    public List<Item> items;

    [Header("Proficiencies")]
    public List<Proficiency> proficiencies;

    #endregion

    private void Start()
    {
        currentWeapon = weapons[0];
        
        CalculateMaxCurrentStats();
        
        SetDamageMod();
        SetArmourClass();
    }

    #region Public Methods

    public void DamageCharacter(Character attacker, Character defender)
    {
        uint incomingDamage = attacker.statDamageBonus
            + attacker.currentWeapon.damage;
        uint damageReduction = defender.armourClass;
        uint baseResult = incomingDamage - damageReduction;

        uint damageResult = (uint)Mathf.Clamp(baseResult, 1, 999);

        defender.currentHP -= damageResult;
        print(attacker.name + " attacked " + defender.name + " dealing "
            + damageResult + " damage! ");
    }

    public virtual void CalculateMaxCurrentStats()
    {
        Debug.LogError("Base form, requires override!");
    }

    #endregion

    #region Private Methods

    public void SetArmourClass()
    {
        armourClass = bodyArmour.armourClassBonus
            + headArmour.armourClassBonus
            + miscarmourBonus;
    }

    private uint ReturnGreatestMeleeStat()
    {
        uint value = strength;
        if (dexterity > value) { value = dexterity; }

        return value;
    }

    private uint ReturnGreatestMagicStat()
    {
        uint value = intelligence;
        if (wisdom > value) { value = wisdom; }

        return value;
    }

    private void SetDamageMod()
    {
        switch (currentWeapon.damageType)
        {
            case DamageTypes.bludgeoning:
                statDamageBonus = ReturnGreatestMeleeStat();
                
                break;
            case DamageTypes.slashing:
                statDamageBonus = ReturnGreatestMeleeStat();

                break;
            case DamageTypes.piercing:
                statDamageBonus = ReturnGreatestMeleeStat();

                break;
            case DamageTypes.divine:
                statDamageBonus = ReturnGreatestMagicStat();

                break;
            case DamageTypes.water:
                statDamageBonus = ReturnGreatestMagicStat();

                break;
            case DamageTypes.fire:
                statDamageBonus = ReturnGreatestMagicStat();

                break;
            case DamageTypes.forest:
                statDamageBonus = ReturnGreatestMagicStat();

                break;
            case DamageTypes.earth:
                statDamageBonus = ReturnGreatestMagicStat();

                break;
            case DamageTypes.thunder:
                statDamageBonus = ReturnGreatestMagicStat();

                break;
            default:
                Debug.LogError("SetDamageMod");
                
                break;
        }
    }

    #endregion
}