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

        public List<Door> doors;

        public RoomTypes roomType;

        List<Enemy> allEnemies = new List<Enemy>();

        List<Loot> allLoot = new List<Loot>();

        public int RoomID;

        private bool doorsDown = false;

        private void Start()
        {
            
        }

        private void Update()
        {
            
            if (roomType == RoomTypes.CloseDoorsUnitlEnemiesDefeated && doorsDown)
            {
                if (AllEnemiesAreDead())
                {
                    MoveDoorsDown(false);
                }
            }
        }

        private bool AllEnemiesAreDead()
        {
            foreach (Enemy enemy in allEnemies)
            {
                if(!enemy.IsDead()) return true;
            }
            return false;
        }

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
                    MoveDoorsDown(true);
                    break;
                case RoomTypes.Boss:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        void MoveDoorsDown(bool down)
        {
            if (!down)
            {
                Debug.Log("Open all Doors");
                doorsDown = false;
                foreach (Door door in doors)
                {
                    door.MoveDoor(down);
                }
            }
            else
            {
                Debug.Log("Close all Doors");
                doorsDown = true;
                foreach (Door door in doors)
                {
                    door.MoveDoor(down);
                }
            }
        }
    }
}