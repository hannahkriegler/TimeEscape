using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TE
{
    /// <summary>
    /// The main class for the loot management
    /// There are different Types of loot which all inherit from this class
    /// </summary>
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

        private bool _savePickedUp;

        private bool _visible;

        private SpriteRenderer[] sprites;
        private Collider2D col;

        private bool _messageShown;

        public Room assignedRoom { get; private set; }

        private void Awake()
        {
            sprites = GetComponentsInChildren<SpriteRenderer>();
            col = GetComponent<Collider2D>();
            assignedRoom = GetComponentInParent<Room>();
            _visible = true;
            if (assignedRoom != null)
                assignedRoom.AddLootToRoom(this);
        }


        public void OnTriggerEnter2D(Collider2D player)
        {
            if (!player.CompareTag("Player")) return;

            PickUpLoot();
        }


        protected virtual void PickUpLoot()
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
                        if (!_messageShown)
                        {
                            _messageShown = true;
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

            if (!doHide) return;
            SoundManager.instance.PlayPickup();
            Show(false);
        }

        public void HandleTimeStamp()
        {
            _savePickedUp = !_visible;
        }

        public void HandleTimeTravel()
        {
            switch (spawnType)
            {
                case SpawnTypes.ReSpawn:
                    Show(!_savePickedUp);
                    break;
                case SpawnTypes.NoRespawn:
                    break;
            }
        }

        protected virtual void CustomBehavior()
        {
            Debug.Log("Picked Up Gem");
        }


        private void Show(bool b)
        {
            col.enabled = b;
            foreach (SpriteRenderer rend in sprites)
            {
                rend.enabled = b;
            }
            _visible = b;
        }
    }
}
