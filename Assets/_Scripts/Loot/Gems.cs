using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gems : Loot
{
    public enum GemTypes
    {
        SwordBuff, // +2 sec for hits on enemies
        LessSkillCosts, // reduces skill cost
        LessTimeMagicCosts, // reduces time magic costs
        IncreasePlayerSpeed, // Player moves slightly faster
        IncreasePlayerJump, // Player can jum slightly higher
        ReduceDamage, // Player loses less time if he gets hit from enemy
        CrazyGem, // Doubles Damage, Doubles countdown speed
        DamageDash, // Dash makes damage
        FasterSword // Sword is faster
    }
    

    public GemTypes gemType;

    
}
