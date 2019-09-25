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
        public GameObject pauseScreen;
        public GameObject systemMessage;
        public GameObject dashIcon;
        public GameObject jumpIcon;

        float gameOverTimer;
        bool gameOver;
        public bool gameIsPaused { get; private set; }


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
            gameIsPaused = false;
            Time.timeScale = 1;
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
                if (gameOverTimer <= 0)
                    SceneManager.LoadScene(0);
            }

            if (cancelMessage)
            {
                if (inputManager.SomethingWasPressed())
                    systemMessage.SetActive(false);
            }

            TutorialTimeTravel();
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

        bool textBoxOpen;
        public void PausePressed()
        {
            if (textBoxOpen)
                return;

            Pause(!gameIsPaused);
            pauseScreen.SetActive(gameIsPaused);
        }

        public void NextButtonPressed()
        {
            if (!textBoxOpen)
                return;

            if (Time.realtimeSinceStartup - textBoxTime < 0.5f)
                return;

            CloseTextBox();
        }

        public void Pause(bool pause = true)
        {
            if (pause)
            {
                Time.timeScale = 0;
                gameIsPaused = true;
            }
            else
            {
                Time.timeScale = 1;
                gameIsPaused = false;
            }
        }

        bool tutorialTimeTravelTriggered;
        void TutorialTimeTravel()
        {
            if (tutorialTimeTravelTriggered)
                return;

            if (!session.canTimeTravel)
                return;

            if (player.TimeSkills.firstTimeTravel)
            {
                tutorialTimeTravelTriggered = true;
                return;
            }

            if (timeLeft <= 60)
            {
                ChangeInfoTextSprite("XboxOne_Y");
                ShowTextBox("Halte <sprite name=\"XboxOne_Y\"> gedrückt um in der Zeit zurückzureisen." +
                    "So gewinnst du deine verlorene Zeit zurück, aber behälst deine Upgrades!");
                tutorialTimeTravelTriggered = true;
            }
        }

        bool cancelMessage;
        float textBoxTime;
        public void ShowTextBox(string message)
        {
            textBoxTime = Time.realtimeSinceStartup;
            systemMessage.SetActive(true);
            systemMessage.GetComponentInChildren<TextMeshProUGUI>().text = message;
            Pause(true);
            textBoxOpen = true;
        }

        public void ChangeInfoTextSprite(string spriteName)
        {
            TMP_SpriteAsset spriteAsset = Resources.Load<TMP_SpriteAsset>("Controller/" + spriteName);
            systemMessage.GetComponentInChildren<TextMeshProUGUI>().spriteAsset = spriteAsset;           
        }

        public void CloseTextBox()
        {
            systemMessage.SetActive(false);
            Pause(false);
            textBoxOpen = false;
        }

        public bool IsTextBoxOpen()
        {
            return textBoxOpen;
        }

        public float CalculateAmbientPitch()
        {
            if (timeLeft > startTime * 0.9f)
                return 1;

            return 1 + (startTime * 0.9f - timeLeft) / (startTime * 0.8f);
        }
    }


}
