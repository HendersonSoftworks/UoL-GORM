using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum spellMod { intelligence, wisdom, charisma }

public class Character : MonoBehaviour
{
    #region Config

    [Header("System setup")]
    [SerializeField]
    private GameObject deathEffect;

    #endregion

    #region Character Sheet

    [Header("Identity")]
    public string[] names;
    public string displayName;
    public Sprite portrait;

    [Header("Max & Current Stats")]
    public uint maxHP;
    public int currentHP;
    public float maxStamina;
    public float currentStamina;
    public uint maxMana;
    public uint currentMana;
    
    [Header("Modifiers")]
    public int moveSpeedBonus;
    public uint meleeDamageBonus;
    public uint miscDamageBonus;
    public uint magicDamageBonus;
    public uint armourClass;
    public uint miscarmourBonus;
    public bool isInvulnerable;
    
    [Header("Statblock")]
    public uint strength;
    public uint dexterity;
    public uint constitution;
    public uint intelligence;
    public uint wisdom;
    public uint charisma;

    [Header("Gear")]
    public Weapon currentWeapon;
    public List<Weapon> weapons = new List<Weapon>();
    public Armour headArmour;
    public Armour bodyArmour;
    public List<Ring> rings = new List<Ring>();

    [Header("Spellcasting")]
    public List<Spell> spells;

    [Header("Items")]
    public List<Item> items;

    [Header("Proficiencies")]
    public List<Proficiency> proficiencies;

    #endregion

    private void Start()
    {                
        SetCharacterMods();
    }

    private void Update()
    {
        ManageHealth();
    }

    #region Public Methods

    public void SetCharacterMods()
    {
        SetMeleeDamageMod();
        SetMagicDamageMod();
        SetArmourClass();
    }

    public void HealCharacter(Character character, float value)
    {
        if (character == null) { return; }
        if (character.currentHP >= character.maxHP) { return; }

        character.currentHP += (int)value;
    }

    public void DamageCharacter(Character attacker, Character defender, Spell spell)
    {
        if (attacker == null) { return; }
        if (defender == null) { return; }
        if (isInvulnerable) { return; }

        uint incomingDamage = (uint)spell.effectValue
            + attacker.magicDamageBonus;
        uint damageReduction = defender.armourClass;
        uint baseResult = incomingDamage - damageReduction;

        uint damageResult = (uint)Mathf.Clamp(baseResult, 1, 999);

        defender.currentHP -= (int)damageResult;
        
        print(attacker.name + " attacked " + defender.name + " dealing "
            + damageResult + " damage! ");
        
    }

    public void DamageCharacter(Character attacker, Character defender)
    {
        if (attacker == null) { return; }
        if (defender == null) { return; }
        if (isInvulnerable) { return; }

        uint incomingDamage = attacker.meleeDamageBonus
            + attacker.currentWeapon.damage;
        uint damageReduction = defender.armourClass;
        uint baseResult = incomingDamage - damageReduction;

        uint damageResult = (uint)Mathf.Clamp(baseResult, 1, 999);

        PlayerAttackController playerAttackController = defender.GetComponent<PlayerAttackController>();
        if (playerAttackController != null && defender.currentStamina > 0)
        {
            if (playerAttackController.isDefending)
            {
                defender.currentStamina -= damageResult;
                print(attacker.name + " attacked " + defender.name + " dealing "
                    + damageResult + " of stamina! ");

                return;
            }
        }

        defender.currentHP -= (int)damageResult;
        print(attacker.name + " attacked " + defender.name + " dealing "
            + damageResult + " damage! ");

        if (tag == "Player")
        {
            StartCoroutine(StartInvulnerableTimer());
        }
        
        // Change anim state if player killed - should really be in EnemyCharacter class
        // Bug - anim not changing...
        EnemyAnimationController enemyAnimationController = 
            attacker.gameObject.GetComponent<EnemyAnimationController>();
        if (defender.currentHP <= 0 && enemyAnimationController != null)
        {
            enemyAnimationController.SetState(EnemyMovementController.MoveStates.idle);
        }
    }

    public virtual void CalculateMaxCurrentStats()
    {
        Debug.LogError("Base form, requires override!");
    }

    public void SetSpriteAlpha(float alpha)
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Color newCol = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);
        sr.color = newCol;
    }

    public IEnumerator StartInvulnerableTimer(float timer = 1)
    {
        isInvulnerable = true;
        //SetSpriteAlpha(127.5f);
        yield return new WaitForSeconds(timer);
        //SetSpriteAlpha(255);
        isInvulnerable = false;
    }

    #endregion

    #region Private Methods

    protected void ManageHealth()
    {
        currentHP = Mathf.Clamp(currentHP, 0, 999);

        if (currentHP <= 0)
        {
            KillCharacter();
        }
    }

    private void KillCharacter()
    {
        var effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
        effect.GetComponent<SpriteRenderer>().color = Color.red;

        Destroy(gameObject);
    }

    private void SetArmourClass()
    {
        uint bodyArmourBonus = 0;
        uint headArmourBonus = 0;

        if (bodyArmour != null) { bodyArmourBonus = bodyArmour.armourClassBonus; }
        if (headArmour != null) { headArmourBonus = headArmour.armourClassBonus; }

        armourClass = bodyArmourBonus
            + headArmourBonus
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

    public void SetMagicDamageMod() // getcomponent every frame causes unnecessary overhead - change!
    {
        var currentSpell = GetComponent<PlayerAttackController>().currentSpellSelected;

        if (currentSpell == null) { return; }

        switch (currentSpell.damageType)
        {
            case DamageTypes.divine:
                magicDamageBonus = ReturnGreatestMagicStat();
                break;
            case DamageTypes.water:
                magicDamageBonus = ReturnGreatestMagicStat();

                break;
            case DamageTypes.fire:
                magicDamageBonus = ReturnGreatestMagicStat();

                break;
            case DamageTypes.forest:
                magicDamageBonus = ReturnGreatestMagicStat();

                break;
            case DamageTypes.earth:
                magicDamageBonus = ReturnGreatestMagicStat();

                break;
            case DamageTypes.thunder:
                magicDamageBonus = ReturnGreatestMagicStat();

                break;

            default:
                Debug.LogError("SetMagicDamageMod");

                break;
        }
    }

    private void SetMeleeDamageMod()
    {
        switch (currentWeapon.damageType)
        {
            case DamageTypes.bludgeoning:
                meleeDamageBonus = ReturnGreatestMeleeStat();
                
                break;
            case DamageTypes.slashing:
                meleeDamageBonus = ReturnGreatestMeleeStat();

                break;
            case DamageTypes.piercing:
                meleeDamageBonus = ReturnGreatestMeleeStat();

                break;
            default:
                Debug.LogError("SetDamageMod");
                
                break;
        }
    }

    #endregion
}