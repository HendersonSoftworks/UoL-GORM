using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    #region Character Sheet
    // Identity
    public string[] names;
    public string displayName;
    public Sprite portrait;

    // HP & AC
    public uint maxHP;
    public int currentHP;
    public uint ac;

    // Statblock
    public uint strength;
    public uint dexterity;
    public uint constitution;
    public uint intelligence;
    public uint wisdom;
    public uint charisma;

    // Gear
    public List<Weapon> weapons;
    public List<Armour> armour;

    // SpellBook
    public List<Spell> spells;

    // Items
    public List<Item> items;

    // Proficiencies
    public List<Proficiency> proficiencies;
    #endregion

    #region Engine logic
    public bool conversing;
    #endregion
}