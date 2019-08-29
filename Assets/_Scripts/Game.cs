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
        public float worldTimeScale { get; private set; }
        public float countdownTimeScale { get; private set; }

        public static bool portalIsSet = false; // Test Dummy

        public InputManager inputManager;
        public Player player { get => inputManager.player; }
       
        public Session session;
        public TimeStorage timeStorage { get; private set; }
        public static Game instance;
        
        public static float TimeLeft;
        [Range(0,4)]
        public static int TimeShardCounter;

        [Header("Data")]
        public Room[] allRooms;
        public GameObject timeStampPrefab;

        private void Awake()
        {
            instance = this;
            TimeLeft = 600;
            TimeShardCounter = 0;
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            session = new Session(this);
            timeStorage = new TimeStorage(this);
            playerTimeScale = 1;
            worldTimeScale = 1;
            countdownTimeScale = 1;
            inputManager.Init(this);
            portalIsSet = true; // TODO: Just for testing, need to be set false after portal setter is implemented
            SetUpRooms();
        }

        private void Update()
        {
            UpdateTime();
        }


        private void SetUpRooms()
        {
            foreach (Room room in allRooms)
            {
                room.SpawnLoot();
            }
        }

        public void GameOver()
        {
            SceneManager.LoadScene(0);
        }
        
        void UpdateTime()
        {
            TimeLeft -= Time.deltaTime * countdownTimeScale;
            if (TimeLeft <= 0)
                GameOver();
        }


        public static void IncreaseTime(float time)
        {
            TimeLeft += time;
        }

        public static void DecreaseTime(float time)
        {
            TimeLeft -= time;
        }

        public static void AddZeitsplitter()
        {
            if (TimeShardCounter < 4) TimeShardCounter++;
        }

        public static void SetZeitsplitterCounter(int counter)
        {
            TimeShardCounter = counter;
        }
        
    }
    
    
}
