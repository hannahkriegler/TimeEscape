using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

namespace TE
{
    public class Game : MonoBehaviour
    {
        //Custom Deltas
        [HideInInspector]
        public float playerTimeScale = 1;

        [HideInInspector]
        public float worldTimeScale = 1;

        [HideInInspector]
        public float countDownScale = 1;


        public InputManager inputManager;
        public Player player { get => inputManager.player; }

        public Session session;
        public TimeStorage timeStorage { get; private set; }
        public static Game instance;
        public float startTime = 600;
        public float timeDrainMultiplier = 1;
        public float timeLeft;
        [Range(0, 4)]
        public int timeShardCounter;

        [Header("Cheats")]
        public bool allMovementSkills;
        public bool unlimitedTimeTravel;
        public bool allTimeSkills;

        [Header("Data")]
        public Room[] allRooms;
        public GameObject timeStampPrefab;

        [Header("Gem stuff")]
        public int timeBonusOnHit;

        [Header("References")]
        public GameObject lootInfo;

        public GameObject systemMessage;


        float gameOverTimer;
        bool gameOver;


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
            inputManager.Init(this);
        }

        private void Update()
        {
            UpdateTime();

            if (Input.GetKey(KeyCode.R) && Input.GetKey(KeyCode.T))
                GameOver();

            if (Input.GetKey(KeyCode.I) && Input.GetKey(KeyCode.O) && Input.GetKey(KeyCode.P))
            {
                allMovementSkills = true;
                unlimitedTimeTravel = true;
                allTimeSkills = true;
            }

            if (gameOver)
            {
                gameOverTimer -= Time.deltaTime;
                if(gameOverTimer <= 0)
                    SceneManager.LoadScene(0);
            }

            if (cancelMessage)
            {
                if(inputManager.SomethingWasPressed())
                    systemMessage.SetActive(false);
        }
        }

        public void GameOver()
        {
            if (gameOver)
                return;

            player.GameOver();
            gameOver = true;
            gameOverTimer = 1.1f;
        }

        void UpdateTime()
        {
            if (timeLeft > 0)
                timeLeft -= Time.deltaTime * timeDrainMultiplier * countDownScale;
            else
            {
                timeLeft = 0;
                GameOver();
            }
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

        bool cancelMessage;
        public void ShowInfo(string message, float duration = 8.0f, bool canCancel = true)
        {
            systemMessage.SetActive(true);
            systemMessage.GetComponentInChildren<TextMeshProUGUI>().text = message;
            StopAllCoroutines();
            StartCoroutine(CloseMessage(duration));
            cancelMessage = canCancel;
        }

        IEnumerator CloseMessage(float duration)
        {
            yield return new WaitForSeconds(duration);
            systemMessage.SetActive(false);
        }
    }


}
