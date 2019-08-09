using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace  TE
{
    public class Game : MonoBehaviour
    {
        //Custom Deltas
        public float playerTimeScale { get; private set; }
        public float worldTimeScale { get; private set; } 
        

        public InputManager inputManager;
        [HideInInspector] public Player player;

        public Session session;
        public TimeStorage storage;


        public static Game instance;
        
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
            inputManager.Init(this);
        }
    }
}
