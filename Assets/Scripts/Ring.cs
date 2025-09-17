using UnityEngine;

public class Ring : Item
{
    public enum ringEffects { none, ac, damage, moveSpeed, attackSpeed, castSpeed, torchLight, mana}
    public ringEffects ringEffect = ringEffects.none;
    public float bonus;
}
