using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    public string spellName;
    public string description;
    
    virtual public void Cast(Character character)
    {
        print("Base behaviour");
    }
}
