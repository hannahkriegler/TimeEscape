using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public abstract class Rooms : MonoBehaviour
{
    public enum RoomTypes
    {
        Spawn,
        Tutorial,
        Loot,
        Mob,
        Normal,
        Boss
    };

    public RoomTypes roomTypesType;

    public Enemy[] allEnemies;
    [HideInInspector]
    public Enemy[] aliveEnemies;

    public Loot[] allLoot;
    [HideInInspector]
    public Loot[] collectedLoot;

    protected Rooms(RoomTypes roomTypesType, Enemy[] allEnemies, Enemy[] aliveEnemies, Loot[] allLoot, Loot[] collectedLoot)
    {
        this.roomTypesType = roomTypesType;
        this.allEnemies = allEnemies;
        this.aliveEnemies = aliveEnemies;
        this.allLoot = allLoot;
        this.collectedLoot = collectedLoot;
    }

    public void OnTriggerEnter2D(Collider2D player)
    {
        if (player.CompareTag("Player"))
        {
            RoomBehaviour();
        }
    }

    public virtual void RoomBehaviour()
    {
        Debug.Log("Missing Behaviour");
    }
    
    
}
