using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TE
{
    public class Game : MonoBehaviour
    {
        //Custom Deltas
        public float playerTimeScale { get; private set; }
        public float worldTimeScale { get; set; }
       

        public InputManager inputManager;
        public Player player { get => inputManager.player; }
       
        public Session session;
        public TimeStorage timeStorage { get; private set; }
        public static Game instance;
        public float startTime = 600;
        public float timeDrainMultiplier = 1;
        public float timeLeft;
        [Range(0,4)]
        public int timeShardCounter;

        [Header("Cheats")]
        public bool allMovementSkills;
        public bool unlimitedTimeTravel;

        [Header("Data")]
        public Room[] allRooms;
        public GameObject timeStampPrefab;

        [Header("Gem stuff")] 
        public int timeBonusOnHit;

        [Header("References")]
        public GameObject lootInfo;
        

        private void Awake()
        {
            instance = this;
            timeLeft = startTime;

            //Start with sword
            timeShardCounter = 4;
        }

        private void Start()
        {
            session = new Session(this);
            timeStorage = new TimeStorage(this);
            playerTimeScale = 1;
            worldTimeScale = 1;
            inputManager.Init(this);
        }

        private void Update()
        {
            UpdateTime();

            if (Input.GetKey(KeyCode.R) && Input.GetKey(KeyCode.T))
                GameOver();

            if(Input.GetKey(KeyCode.I) && Input.GetKey(KeyCode.O) && Input.GetKey(KeyCode.P))
            {
                allMovementSkills = true;
                unlimitedTimeTravel = true;
            }
        }

        public void GameOver()
        {
            SceneManager.LoadScene(0);
        }
        
        void UpdateTime()
        {
            timeLeft -= Time.deltaTime * timeDrainMultiplier;
            if (timeLeft <= 0)
                GameOver();
        }


        public void IncreaseTime(float time)
        {
            timeLeft += time;
        }

        public void DecreaseTime(float time)
        {
            timeLeft -= time;
        }

        public bool AddTimeShard()
        {
            if (timeShardCounter < 4)
            {
                timeShardCounter++;
                return true;
            }
            return false;
        }

        public void SetTimeShardCounter(int count)
        {
            timeShardCounter = count;
        }

        public bool CanTimeTravel()
        {
            return timeShardCounter == 4 || unlimitedTimeTravel;
        }


        public void HandleTimeStampEnemies()
        {
            foreach (Room room in allRooms)
            {
                room.HandleTimeStamp();
            }
        }

        public void HandleTimeTravelEnemies()
        {
            foreach (Room room in allRooms)
            {
                room.HandleTimeTravel();
            }
        }
        
    }
    
    
}
