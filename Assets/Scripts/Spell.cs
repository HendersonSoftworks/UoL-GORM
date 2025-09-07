using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : Item
{
    public enum SpellEffects { damage, buff }
    public SpellEffects effect;
    public enum DamageTypes { none, force, fire, radiant, cold }
    public DamageTypes damageType;
    public float effectValue;
    public Character originCharacter;

    private Rigidbody2D rb;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Cast(Character originCharacter)
    {
        //print("Cast by " + originCharacter.name);
        //switch (damageType)
        //{
        //    case DamageTypes.none:
        //        break;
        //    case DamageTypes.force:
        //        break;
        //    case DamageTypes.fire:
        //        break;
        //    case DamageTypes.radiant:
        //        break;
        //    case DamageTypes.cold:
        //        break;
        //    default:
        //        break;
        //}
    }

    private void Update()
    {
        switch (damageType)
        {
            case DamageTypes.none:
                break;
            case DamageTypes.force:
                break;
            case DamageTypes.fire:
                rb.linearVelocity = Vector2.up * Time.deltaTime * 50;
                break;
            case DamageTypes.radiant:
                break;
            case DamageTypes.cold:
                break;
            default:
                break;
        }
    }
}
