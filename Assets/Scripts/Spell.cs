using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Spell : Item
{
    public enum SpellEffects { damage, buff }
    public enum buffTypes { none, health, stamina, resistance }

    [Header("Spell Config")]
    public GameObject explosionPrefab;
    public SpellEffects effect;
    public DamageTypes damageType;
    public buffTypes buffType;
    public float effectValue;
    public float castValue;
    public Character casterCharacter;

    private Rigidbody2D rb;

    [Header("Debug")]
    public Vector2 moveAngle;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Cast(Character originCharacter, Vector2 direction, Character targetCharacter = null)
    {
        casterCharacter = originCharacter;

        originCharacter.SetMagicDamageMod();

        if (casterCharacter.currentMana < castValue)
        {
            print(casterCharacter.name + ": Not enough mana");
            return;
        }
        
        casterCharacter.currentMana -= (uint)castValue;

        var tempSpell = Instantiate(gameObject, originCharacter.transform.position, Quaternion.identity);
        
        tempSpell.GetComponent<Spell>().moveAngle = direction;
        if (originCharacter.tag == "Enemy") { tempSpell.tag = "eHitboxSpell"; }
    }

    public void SpellHitEffects(Collider2D collision=null)
    {
        if (collision == null)
        {
            InstantiateSpellExplosion();

            Destroy(gameObject);

            return;
        }

        if (tag == "eHitboxSpell" && collision.tag == "Enemy")
        {
            return;
        }

        if ((tag == "eHitbox" && collision.tag == "Player") || 
            (collision.tag == "Enemy" || 
            collision.tag == "walls"))
        {
            InstantiateSpellExplosion();

            Destroy(gameObject);
        }
    }

    private void InstantiateSpellExplosion()
    {
        var tempExp = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        tempExp.GetComponent<SpriteRenderer>().color = this.GetComponent<SpriteRenderer>().color;
        tempExp.GetComponent<Light2D>().color = this.GetComponent<SpriteRenderer>().color;
    }

    private void FixedUpdate()
    {
        switch (effect)
        {
            case SpellEffects.damage:
                rb.linearVelocity = moveAngle * Time.deltaTime * 500;

                break;
            case SpellEffects.buff:
                switch (buffType)
                {
                    case buffTypes.none:
                        break;
                    case buffTypes.health:
                        casterCharacter.HealCharacter(casterCharacter, effectValue);
                        InstantiateSpellExplosion();
                        Destroy(gameObject);
                        break;
                    case buffTypes.stamina:
                        break;
                    case buffTypes.resistance:
                        break;
                    default:
                        break;
                }
                break;
            default:
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SpellHitEffects(collision);
    }
}
