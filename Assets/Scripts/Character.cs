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

    [Header("HP & AC")]
    public uint maxHP;
    public int currentHP;
    public uint ac;

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
}