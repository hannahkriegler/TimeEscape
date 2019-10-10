using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace TE
{
    public class Room : MonoBehaviour, ITimeTravel
    {
        public enum RoomTypes
        {
            Normal,
            CloseDoorsUnitlEnemiesDefeated,
            Boss,
            SpecialTimeRoom
        };

        public List<Door> doors;

        public RoomTypes roomType;

        List<Enemy> allEnemies = new List<Enemy>();

        List<Loot> allLoot = new List<Loot>();

        public int RoomID;

        protected bool doorsDown = false;

        private bool saveDoorsDown;

        [Header("Special Rooms Infos")]
        public int secondsLeftToOpen;

        public GameObject finalBoss;

        private void Start()
        {
            //Debug.Log("Mob Room has " + allEnemies.Count + " Enemies");
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
            if (roomType == RoomTypes.SpecialTimeRoom && !doorsDown
                                                      && Game.instance.timeLeft <= secondsLeftToOpen)
            {
                MoveDoorsDown(true);
            }
        }

        private bool AllEnemiesAreDead()
        {
            foreach (Enemy enemy in allEnemies)
            {
                //Debug.Log("Enemy has " + enemy.hitPoints + " hitPoints");
                if (!enemy.IsDead())
                {

                    return false;
                }
            }

            Debug.Log("All Dead");
            return true;
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
            foreach (Door door in doors)
            {
                if (door != null)
                    door.HandleTimeTravel();
            }
            if (finalBoss != null)
                finalBoss.GetComponent<FinalBoss>()?.HandleTimeTravel();
            doorsDown = saveDoorsDown;
        }

        public void HandleTimeStamp()
        {
            foreach (Enemy enemy in allEnemies)
            {
                if (enemy != null)
                    enemy.HandleTimeStamp();
            }
            foreach (Loot loot in allLoot)
            {
                if (loot != null)
                    loot.HandleTimeStamp();
            }
            foreach (Door door in doors)
            {
                if (door != null)
                    door.HandleTimeStamp();
            }
            
            if(finalBoss != null)
                finalBoss.GetComponent<FinalBoss>()?.HandleTimeStamp();
            saveDoorsDown = doorsDown;
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
                    if (finalBoss != null) finalBoss.GetComponent<Boss>().setUp = true;
                    break;
                case RoomTypes.Boss:
                    MoveDoorsDown(true);
                    finalBoss.GetComponent<FinalBoss>()?.ActivateBoss();
                    break;
                case RoomTypes.SpecialTimeRoom:
                    if (doorsDown) break; // bei der tür genau inverse
                    Game.instance.ShowTextBox("Verbleibende Zeit Muss kleiner 2 Minuten sein!");
                    break;
            }
        }


        protected void MoveDoorsDown(bool b)
        {
            if (!b)
            {
                Debug.Log("Open all Doors");
                doorsDown = false;
                foreach (Door door in doors)
                {
                    door.MoveDoor(b);
                }
            }
            else
            {
                Debug.Log("Close all Doors");
                doorsDown = true;
                foreach (Door door in doors)
                {
                    door.MoveDoor(b);
                }
            }
        }
    }
}
    