using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemName;
    public string description;
    public bool isConsumable;

    virtual public void Use(Character character)
    {
        print("Base behaviour");
    }
}
