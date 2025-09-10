using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : Item
{
    public enum SpellEffects { damage, buff }
    public SpellEffects effect;
    public enum DamageTypes { none, force, fire, radiant, cold }
    public DamageTypes damageType;
    public enum buffTypes { none, health, stamina, resistance}
    public buffTypes buffType;

    public float effectValue;

    private Rigidbody2D rb;
    private PlayerCharacter playerCharacter;
    [SerializeField]
    private Vector2 moveAngle;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Cast(Character originCharacter, Character targetCharacter = null)
    {
        var tempSpell = Instantiate(gameObject, originCharacter.transform.position, Quaternion.identity);
        tempSpell.transform.SetLocalPositionAndRotation(
            originCharacter.gameObject.transform.position,
            originCharacter.gameObject.transform.localRotation);

        // Convert degrees to quaternion
        Quaternion rotation = Quaternion.Euler(0, 0, originCharacter.gameObject.transform.localRotation.z);

        // Apply to transform
        transform.localRotation = rotation; 
    }

    private void FixedUpdate()
    {
        switch (effect)
        {
            case SpellEffects.damage:
                switch (damageType)
                {
                    case DamageTypes.none:
                        break;
                    case DamageTypes.force:
                        break;
                    case DamageTypes.fire:
                        rb.linearVelocity = Vector2.up * Time.deltaTime * 500;
                        break;
                    case DamageTypes.radiant:
                        break;
                    case DamageTypes.cold:
                        break;
                    default:
                        break;
                }
                break;
            case SpellEffects.buff:
                switch (buffType)
                {
                    case buffTypes.none:
                        break;
                    case buffTypes.health:
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

    //private void Update()
    //{
    //    switch (effect)
    //    {
    //        case SpellEffects.damage:
    //            switch (damageType)
    //            {
    //                case DamageTypes.none:
    //                    break;
    //                case DamageTypes.force:
    //                    break;
    //                case DamageTypes.fire:
    //                    rb.linearVelocity = moveAngle * Time.deltaTime * 500;
    //                    break;
    //                case DamageTypes.radiant:
    //                    break;
    //                case DamageTypes.cold:
    //                    break;
    //                default:
    //                    break;
    //            }
    //            break;
    //        case SpellEffects.buff:
    //            switch (buffType)
    //            {
    //                case buffTypes.none:
    //                    break;
    //                case buffTypes.health:
    //                    break;
    //                case buffTypes.stamina:
    //                    break;
    //                case buffTypes.resistance:
    //                    break;
    //                default:
    //                    break;
    //            }
    //            break;
    //        default:
    //            break;
    //    }
    //}
}
