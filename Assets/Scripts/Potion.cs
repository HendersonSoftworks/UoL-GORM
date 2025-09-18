using UnityEngine;

public class Potion : Item
{
    public enum potionEffects { none, health, mana }
    public potionEffects potionEffect = potionEffects.none;
}
