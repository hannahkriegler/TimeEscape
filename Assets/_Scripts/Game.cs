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
        public float worldTimeScale { get; private set; }
        public float coundwodnTimeScale { get; private set; }

        public static bool portalIsSet = false; // Test Dummy

        public InputManager inputManager;
        public Player player { get => inputManager.player; }
       
        public Session session;
        public TimeStorage storage;
        public static Game instance;
        
        public static float TimeLeft;
        [Range(0,4)]
        public static int ZeitsplitterCounter;

        public Rooms[] allRooms;

        public bool testTimeTravel = false;
        
        private void Awake()
        {
            instance = this;
            TimeLeft = 600;
            ZeitsplitterCounter = 0;
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            session = new Session(this);
            storage = new TimeStorage(this);
            playerTimeScale = 1;
            worldTimeScale = 1;
            coundwodnTimeScale = 1;
            inputManager.Init(this);
            portalIsSet = true; // TODO: Just for testing, need to be set false after portal setter is implemented
            SetUpRooms();
        }

        private void Update()
        {
            if (testTimeTravel)
            {
                SetUpRooms();
                testTimeTravel = false;
            }
        }


        private void SetUpRooms()
        {
            foreach (Rooms room in allRooms)
            {
                room.SpawnLoot();
            }
        }

        public void GameOver()
        {
            SceneManager.LoadScene(0);
        }
        

        private void FixedUpdate()
        {
            TimeLeft -= Time.deltaTime * coundwodnTimeScale;
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
            if (ZeitsplitterCounter < 4) ZeitsplitterCounter++;
        }

        public static void SetZeitsplitterCounter(int counter)
        {
            ZeitsplitterCounter = counter;
        }
        
    }
    
    
}
