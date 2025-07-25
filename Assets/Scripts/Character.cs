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

    [Header("Armour")]
    public uint armourClass;

    [Header("Statblock")]
    public uint strength;
    public uint dexterity;
    public uint constitution;
    public uint intelligence;
    public uint wisdom;
    public uint charisma;

    [Header("Gear")]
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

    #region Public Methods

    public virtual void CalculateMaxCurrentStats()
    {
        // TODO
    }

    #endregion
}