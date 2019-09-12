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
            Boss,
            SpecialTimeRoom
        };

        public List<Door> doors;

        public RoomTypes roomType;

        List<Enemy> allEnemies = new List<Enemy>();

        List<Loot> allLoot = new List<Loot>();

        public int RoomID;

        protected bool doorsDown = false;

        [Header("Special Time Rooms Infos")]
        public int secondsLeftToOpen;
        public GameObject textInfo;

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
                Debug.Log("Enemy has " + enemy.hitPoints + " hitPoints");
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
                case RoomTypes.SpecialTimeRoom:
                    if (doorsDown) break; // bei der tür genau inverse
                    textInfo.SetActive(true);
                    StartCoroutine(HideTextBox());
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

        IEnumerator HideTextBox()
        {
            yield return new WaitForSeconds(2);
            textInfo.SetActive(false);

        }

    }
}