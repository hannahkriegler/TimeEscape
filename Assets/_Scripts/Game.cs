﻿using System;
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
        public float countdownTimeScale { get; private set; }

        public InputManager inputManager;
        public Player player { get => inputManager.player; }
       
        public Session session;
        public TimeStorage timeStorage { get; private set; }
        public static Game instance;
        public float startTime = 600;
        
        public float timeLeft;
        [Range(0,4)]
        public int timeShardCounter;

        [Header("Data")]
        public Room[] allRooms;
        public GameObject timeStampPrefab;

        [Header("Gem stuff")] 
        public int timeBonusOnHit;
        

        private void Awake()
        {
            instance = this;
            timeLeft = startTime;
            timeShardCounter = 0;
        }

        private void Start()
        {
            session = new Session(this);
            timeStorage = new TimeStorage(this);
            playerTimeScale = 1;
            worldTimeScale = 1;
            countdownTimeScale = 1;
            inputManager.Init(this);
        }

        private void Update()
        {
            UpdateTime();
        }

        public void GameOver()
        {
            SceneManager.LoadScene(0);
        }
        
        void UpdateTime()
        {
            timeLeft -= Time.deltaTime * countdownTimeScale;
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

        public void AddTimeShard()
        {
            if (timeShardCounter < 4) timeShardCounter++;
        }

        public void SetTimeShardCounter(int counter)
        {
            timeShardCounter = counter;
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
