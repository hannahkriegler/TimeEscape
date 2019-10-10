using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TE
{
    public class Loot : MonoBehaviour, ITimeTravel
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

        bool visible;

        SpriteRenderer[] sprites;
        Collider2D col;

        bool messageShown;

        public Room assignedRoom { get; private set; }

        private void Awake()
        {
            sprites = GetComponentsInChildren<SpriteRenderer>();
            col = GetComponent<Collider2D>();
            assignedRoom = GetComponentInParent<Room>();
            visible = true;
            if (assignedRoom != null)
                assignedRoom.AddLootToRoom(this);
        }


        public void OnTriggerEnter2D(Collider2D player)
        {
            if (!player.CompareTag("Player")) return;

            PickUpLoot();
        }


        public virtual void PickUpLoot()
        {
            bool doHide = true;
            switch (lootType)
            {
                case LootTypes.Time:
                    Debug.Log("Increased Time");
                    Game.instance.IncreaseTime(addedTime);
                    break;
                case LootTypes.Zeitsplitter:
                    Debug.Log("Picked Up Zeitsplitter");
                    if (Game.instance.timeShardCounter >= 4 && !Game.instance.AllowMoreThan4TimeShards)
                    {
                        doHide = false;
                        if (!messageShown)
                        {
                            messageShown = true;
                            Game.instance.ShowTextBox("Du hast bereits 4 Zeitsplitter");
                        }
                    }
                    else
                        Game.instance.AddTimeShard();
                    break;
                case LootTypes.Gem:
                    CustomBehavior();
                    break;
                case LootTypes.MovementSkills:
                    CustomBehavior();
                    break;
                case LootTypes.ActiveSkills:
                    CustomBehavior();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            if (doHide)
            {
                SoundManager.instance.PlayPickup();
                Show(false);
            }
        }

        public void HandleTimeStamp()
        {
            savePickedUp = !visible;
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
            col.enabled = b;
            foreach (SpriteRenderer rend in sprites)
            {
                rend.enabled = b;
            }
            visible = b;
        }
    }
}
