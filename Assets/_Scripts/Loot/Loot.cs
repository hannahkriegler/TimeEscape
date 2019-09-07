using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TE
{
    public class Loot : MonoBehaviour
    {
        public enum LootTypes
        {
            Time,
            Zeitsplitter,
            Gem
        }

        public enum SpawnTypes
        {
            ReSpawn,
            NoRespawn
        }

        public int addedTime = 10;
        public LootTypes lootType;

        public SpawnTypes spawnType = SpawnTypes.ReSpawn;

        public GameObject textBox;
        bool savePickedUp;

        private void Start()
        {
            //textBox = GameObject.FindGameObjectWithTag("LootInfo");
        }

        public void OnTriggerEnter2D(Collider2D player)
        {
            if (!player.CompareTag("Player")) return;

            PickUpLoot();
        }

        public virtual void PickUpLoot()
        {
            switch (lootType)
            {
                case LootTypes.Time:
                    Debug.Log("Increased Time");
                    Game.instance.IncreaseTime(addedTime);
                    break;
                case LootTypes.Zeitsplitter:
                    Debug.Log("Picked Up Zeitsplitter");
                    Game.instance.AddTimeShard();
                    break;
                case LootTypes.Gem:
                    GemBehaviour();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            gameObject.SetActive(false);
        }

        public void HandleTimeStamp()
        {
            savePickedUp = !gameObject.activeSelf;
        }

        public void HandleTimeTravel()
        {
            switch (spawnType)
            {
                case SpawnTypes.ReSpawn:
                    gameObject.SetActive(!savePickedUp);
                    break;
                case SpawnTypes.NoRespawn:
                    break;
                default:
                    break;
            }
        }

        public virtual void GemBehaviour()
        {
            Debug.Log("Picked Up Gem");
        }
    }
}
