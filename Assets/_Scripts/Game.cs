using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace  TE
{
    public class Game : MonoBehaviour
    {
        //Custom Deltas
        public float deltaPlayer;
        public float deltaWorld;

        public InputManager inputManager;
        [HideInInspector] public Player player;

        public Session session;
        public TimeStorage storage;

        private void Start()
        {
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
