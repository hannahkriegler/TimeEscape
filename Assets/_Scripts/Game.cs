using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace  TE
{
    public class Game : MonoBehaviour
    {
        //Custom Deltas
        public float deltaPlayer { get; private set; }
        public float deltaWorld { get; private set; }

        public InputManager inputManager;
        [HideInInspector] public Player player;

        public Session session;
        public TimeStorage storage;


        public static Game instance;
        
        private void Awake()
        {
            instance = this;
            throw new NotImplementedException();
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            session = new Session(this);
            storage = new TimeStorage(this);
            inputManager.Init(this);
        }

        private void Update()
        {
            deltaPlayer = Time.deltaTime;
            deltaWorld = Time.deltaTime;
        }
    }
}
