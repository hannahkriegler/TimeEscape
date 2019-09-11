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
            MovementSkills,
            ActiveSkills,
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

        bool savePickedUp;

        public Room assignedRoom { get; private set; }

        private void Awake()
        {
            assignedRoom = GetComponentInParent<Room>();
            if(assignedRoom != null)
                assignedRoom.AddLootToRoom(this);
        }


        public void OnTriggerEnter2D(Collider2D player)
        {
            if (!player.CompareTag("Player")) return;

            PickUpLoot();
        }

        public virtual void PickUpLoot()
        {
            bool disableAfterPickup = true;
            switch (lootType)
            {
                case LootTypes.Time:
                    Debug.Log("Increased Time");
                    Game.instance.IncreaseTime(addedTime);
                    break;
                case LootTypes.Zeitsplitter:
                    Debug.Log("Picked Up Zeitsplitter");
                    disableAfterPickup = Game.instance.AddTimeShard();
                    break;
                case LootTypes.Gem:
                    CustomBehavior();
                    break;
                case LootTypes.MovementSkills:
                    CustomBehavior();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            if(disableAfterPickup)
             Show(false);
        }

        public void HandleTimeStamp()
        {
            savePickedUp = !gameObject.GetComponent<SpriteRenderer>().enabled;
            //savePickedUp = !gameObject.activeSelf;
        }

        public void HandleTimeTravel()
        {
            switch (spawnType)
            {
                case SpawnTypes.ReSpawn:
                    Show(!savePickedUp);
                    break;
                case SpawnTypes.NoRespawn:
                    break;
                default:
                    break;
            }
        }

        public virtual void CustomBehavior()
        {
            Debug.Log("Picked Up Gem");
        }

        
        protected void Show(bool b)
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = b;
            gameObject.GetComponent<CircleCollider2D>().enabled = b;
        }
    }
}
