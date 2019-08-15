using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Rooms : MonoBehaviour
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

    public RoomTypes roomType;

    public Enemy[] allEnemies;
    [HideInInspector]
    public Enemy[] aliveEnemies;

    public Loot[] allLoot;
    [HideInInspector]
    public Loot[] collectedLoot;
    
    public int RoomID;

    protected Rooms(RoomTypes roomType, Enemy[] allEnemies, Enemy[] aliveEnemies, Loot[] allLoot, Loot[] collectedLoot)
    {
        this.roomType = roomType;
        this.allEnemies = allEnemies;
        this.aliveEnemies = aliveEnemies;
        this.allLoot = allLoot;
        this.collectedLoot = collectedLoot;
    }

    private void Start()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D player)
    {
        if (!player.CompareTag("Player")) return;

        switch (roomType)
        {
            case RoomTypes.Tutorial:
                TutorialRoomBehaviour();
                break;
            case RoomTypes.Spawn:
                SpawnRoomBehaviour();
                break;
            case RoomTypes.Normal:
                NormalRoomBehaviour();
                break;
            case RoomTypes.Mob:
                MobRoomBehaviour();
                break;
            case RoomTypes.Loot:
                LootRoomBehaviour();
                break;
            case RoomTypes.Boss:
                BossRoomBehaviour();
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }
        
        Debug.Log("Enemies alive: " + aliveEnemies.Length);
        
    }
    

    private void TutorialRoomBehaviour()
    {
        Debug.Log("Welcome to the Tutorial Room!");
    }

    private void SpawnRoomBehaviour()
    {
        Debug.Log(("Welcome to the Spawn Romm!"));
    }

    private void NormalRoomBehaviour()
    {
        Debug.Log("Welcome to a Normal Room!");
    }

    private void MobRoomBehaviour()
    {
        Debug.Log("Welcome to a Mob Room!");
    }

    private void BossRoomBehaviour()
    {
        Debug.Log("Welcome to a Boss Room!");
    }

    private void LootRoomBehaviour()
    {
        Debug.Log("Welcome to a Loot Room!");
    }

    
}
