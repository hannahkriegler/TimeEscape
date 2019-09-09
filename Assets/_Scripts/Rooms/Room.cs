using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace TE
{
    public class Room : MonoBehaviour
    {
        public enum RoomTypes
        {
           Normal,
           CloseDoorsUnitlEnemiesDefeated,
           Boss
        };

        public RoomTypes roomType;

        List<Enemy> allEnemies = new List<Enemy>();

        List<Loot> allLoot = new List<Loot>();

        public int RoomID;

        public void AddEnemyToRoom(Enemy enemy)
        {
            if (!allEnemies.Contains(enemy))
                allEnemies.Add(enemy);
        }

        public void AddLootToRoom(Loot loot)
        {
            if (!allLoot.Contains(loot))
                allLoot.Add(loot);
        }

        public void HandleTimeTravel()
        {
            foreach (Enemy enemy in allEnemies)
            {
                enemy.HandleTimeTravel();
            }
            foreach (Loot loot in allLoot)
            {
                loot.HandleTimeTravel();
            }
        }

        public void HandleTimeStamp()
        {
            foreach (Enemy enemy in allEnemies)
            {
                enemy.HandleTimeStamp();
            }
            foreach (Loot loot in allLoot)
            {
                loot.HandleTimeStamp();
            }
        }


        public void NotifyEnemyDied(Enemy enemy)
        {
        
        }

        public void OnTriggerEnter2D(Collider2D player)
        {
            if (!player.CompareTag("Player")) return;

            switch (roomType)
            {
                case RoomTypes.Normal:
                    break;
                case RoomTypes.CloseDoorsUnitlEnemiesDefeated:
                    break;
                case RoomTypes.Boss:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}