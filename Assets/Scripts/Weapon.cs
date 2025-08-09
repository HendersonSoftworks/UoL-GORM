using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum DamageTypes { bludgeoning, slashing, piercing, 
                          divine, water, fire, forest, earth, thunder,}

public enum WeaponAttributes { Strength, Dexterity, Consitution,
                               Intelligence, Wisdom, Charisma}

public class Weapon : MonoBehaviour
{
    public string weaponName;
    public WeaponAttributes weaponAttribute;
    public uint damage;
    public bool isMagical;
    public DamageTypes damageType;
    public Sprite image;
}
