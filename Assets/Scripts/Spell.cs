using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : Item
{
    public enum SpellEffects { damage, buff }
    public SpellEffects effect;
    public float effectValue;
    
    virtual public void Cast(Character originCharacter)
    {
        print("Cast by " + originCharacter.name);
    }
}
