using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Video;

namespace TE
{
    public class Game : MonoBehaviour
    {
        //Custom Deltas
        [HideInInspector] public float playerTimeScale = 1;

        [HideInInspector] public float worldTimeScale = 1;

        [HideInInspector] public float countDownScale = 1;


        public InputManager inputManager;

        public Player player
        {
            get => inputManager.player;
        }

        public Session session;
        public TimeStorage timeStorage { get; private set; }
        public static Game instance;
        public float startTime = 600;
        public float timeDrainMultiplier = 1;
        public float timeLeft;
        [Range(0, 4)] public int timeShardCounter;

        [Header("Cheats")] public bool allMovementSkills;
        public bool unlimitedTimeTravel;
        public bool allTimeSkills;
        public bool unlockFireBall;
        public bool skipTutorials;
        public bool skipIntro;

        [Header("Configs")]
        public bool AllowMoreThan4TimeShards = true;

        [Header("Data")] public Room[] allRooms;
        public GameObject timeStampPrefab;

        [Header("Gem stuff")] public int timeBonusOnHit;

        [Header("References")] public GameObject lootInfo;
        public GameObject pauseScreen;
        public GameObject systemMessage;
        public GameObject dashIcon;
        public GameObject jumpIcon;
        public GameObject fireBallIcon;
        public GameObject rewindIcon;
        public GameObject wonPanel;
        public TextMeshProUGUI timeleftText;
        public GameObject ui;

        [Header("Video")]
        public VideoPlayer videoPlayer;
        public VideoClip intro;
        public VideoClip outro;
        bool videoPlaying;

        float gameOverTimer;
        bool gameOver;
        public bool gameIsPaused { get; private set; }

        public Healthbar bossHealthBar;

        private bool wonGame;

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

            if (skipTutorials)
            {
                session.UnlockTimeTravel();
                session.UnlockTimestamp();
            }

            if (!skipIntro)
            {
                ui.SetActive(false);
                videoPlayer.clip = intro;
                videoPlayer.Play();
                Time.timeScale = 0;
                videoPlaying = true;
            }
        }




        private void Update()
        {
            if (videoPlaying)
            {
                if (videoPlayer.isPrepared && !videoPlayer.isPlaying)
                {
                    videoPlayer.Stop();
                    Time.timeScale = 1;
                    videoPlaying = false;
                    ui.SetActive(true);
                }
            }

            UpdateTime();

            Cheats();

            if (Input.GetKey(KeyCode.R) && Input.GetKey(KeyCode.T))
                GameOver();

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

        void Cheats()
        {
            if (Application.isEditor)
            {
                if (Input.GetKey(KeyCode.Alpha1))
                    timeLeft = 60;
                if (Input.GetKey(KeyCode.Alpha2))
                    timeLeft = 120;
                if (Input.GetKey(KeyCode.Alpha3))
                    timeLeft = 180;
                if (Input.GetKey(KeyCode.Alpha4))
                    timeLeft = 240;

                if (Input.GetKeyDown(KeyCode.T))
                    AddTimeShard();
                
                if (Input.GetKey(KeyCode.P))
                {
                    allMovementSkills = true;
                    unlimitedTimeTravel = true;
                    allTimeSkills = true;
                }
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
            if (wonGame)
                return;
            
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
            player.stopWatch.TimeChange(Mathf.RoundToInt(time));
            timeLeft += time;
        }

        public void DecreaseTime(float time)
        {
            player.stopWatch.TimeChange(-Mathf.RoundToInt(time));
            timeLeft -= time;
        }

        public bool AddTimeShard()
        {
            if (timeShardCounter < 4 || AllowMoreThan4TimeShards)
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

        public void ReduceTimeShardCounter(int count)
        {
            timeShardCounter -= count;
            if (timeShardCounter < 0)
                timeShardCounter = 0;
        }

        public bool CanTimeTravel()
        {
            return timeShardCounter >= 4 || unlimitedTimeTravel;
        }


        public void HandleTimeStampEnemies()
        {
            foreach (Room room in allRooms)
            {
                room?.HandleTimeStamp();
            }
        }

        public void HandleTimeTravelEnemies()
        {
            foreach (Room room in allRooms)
            {
                room?.HandleTimeTravel();
            }
        }

        bool textBoxOpen;

        public void PausePressed()
        {
            if (textBoxOpen)
                return;

            if (wonGame)
            {
                SceneManager.LoadScene(0);
                return;
            }

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

            if (timeLeft <= 60 && !skipTutorials)
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
            float res = 1;
            if (timeLeft <= startTime * 0.9f)
                res = 1 + (startTime * 0.9f - timeLeft) / (startTime * 0.8f);
            res *= worldTimeScale;
            return res;
        }

        public void Won()
        {
            wonPanel.SetActive(true);
            timeleftText.text = "Zeit übrig: " + Mathf.Round(timeLeft) + " Sekunden.";
            wonGame = true;
            StartCoroutine(PlayOutro());
        }

        IEnumerator PlayOutro()
        {
            yield return new WaitForSeconds(1.2f);
            ui.SetActive(false);
            videoPlayer.clip = outro;
            videoPlayer.Play();
            Time.timeScale = 0;
            videoPlaying = true;
        }
    }
}