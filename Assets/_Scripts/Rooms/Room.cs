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
            Spawn,
            Tutorial,
            Loot,
            Mob,
            Normal,
            Boss
        };

        public RoomTypes roomType;

        public Enemy[] allEnemies;
        List<Enemy> aliveEnemies;

        public Loot[] allLoot;
        [HideInInspector]
        public Loot[] collectedLoot;

        public int RoomID;

        private void Start()
        {
            aliveEnemies = new List<Enemy>();
            aliveEnemies.AddRange(allEnemies);

            foreach (Enemy enemy in allEnemies)
            {
                enemy.AssignRoom(this);
            }
        }

        public void SpawnLoot()
        {
            foreach (Loot loot in allLoot)
            {
                if (loot.spawnType == Loot.SpawnTypes.Spawn ||
                    loot.spawnType == Loot.SpawnTypes.ReSpawn)
                {
                    loot.ShowSprite();
                }

                if (loot.spawnType == Loot.SpawnTypes.NotSpawn)
                {
                    loot.HideSprite();
                }
            }
        }

        public void RespawnEnemies()
        {
            foreach (Enemy enemy in allEnemies)
            {
                //TODO Handling Time State
            }
        }

        public void ChangeLootFromReSpawnToNotSpawn()
        {
            foreach (Loot loot in allLoot)
            {
                if (loot.spawnType == Loot.SpawnTypes.ReSpawn)
                {
                    loot.spawnType = Loot.SpawnTypes.NotSpawn;
                }
            }
        }

        public void NotifyEnemyDied(Enemy enemy)
        {
            if (aliveEnemies.Contains(enemy))
                aliveEnemies.Remove(enemy);
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

            Debug.Log("Enemies alive: " + aliveEnemies.Count);
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
}