using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace  TE
{
    public class Game : MonoBehaviour
    {
        //Custom Deltas
        public float playerTimeScale { get; private set; }
        public float worldTimeScale { get; private set; }
        public float coundwodnTimeScale { get; private set; }
        

        public InputManager inputManager;
        [HideInInspector] public Player player;

        public Session session;
        public TimeStorage storage;


        public static Game instance;

        public Countdown countdown;
        
        private void Awake()
        {
            instance = this;
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
            countdown.Init(this);
        }

        public void GameOver()
        {
            SceneManager.LoadScene(0);
        }
    }
}
