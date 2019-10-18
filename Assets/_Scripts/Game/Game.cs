using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Video;

namespace TE
{
    /// <summary>
    /// Handles Loose/Win Conditions, Time Travel, User Interface, Video Playback
    /// </summary>
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

        //Variables for testing
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
        public Crazy crazy;

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


        /// <summary>
        /// Stops Vide Playback
        /// </summary>
        void StopVideo()
        {
            videoPlayer.Stop();
            Time.timeScale = 1;
            videoPlaying = false;
            ui.SetActive(true);
        }

        private void Update()
        {
            if (videoPlaying)
            {
                if (videoPlayer.isPrepared && !videoPlayer.isPlaying)
                {
                    StopVideo();
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

        /// <summary>
        /// Handle cheat button Inputs to change current time, add a timeshard or to unlock all skills.
        /// </summary>
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

        /// <summary>
        /// Triggers Games Game Over State. Game restarts after a delay to show the character die animation.
        /// </summary>
        public void GameOver()
        {
            if (gameOver)
                return;

            player.GameOver();
            gameOver = true;
            gameOverTimer = 1.1f;
        }

        /// <summary>
        /// Updates the countdown. If countdown is zero triggers Game Over State
        /// </summary>
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

        /// <summary>
        /// Increases countdown by specified amount. Triggers event to show clock above players head
        /// </summary>
        /// <param name="time">Amount of time added in seconds</param>
        public void IncreaseTime(float time)
        {
            player.stopWatch.TimeChange(Mathf.RoundToInt(time));
            timeLeft += time;
        }

        /// <summary>
        /// Decreases countdown by specified amount. Triggers event to show clock above players head
        /// </summary>
        /// <param name="time">Amount of time decreased in seconds</param>
        public void DecreaseTime(float time)
        {
            player.stopWatch.TimeChange(-Mathf.RoundToInt(time));
            timeLeft -= time;
        }

        /// <summary>
        /// Adds a timeshard. Checks whether already 4 shards are collected.
        /// When variable AllowMoreThan4TimeShards is true allow more than 4 shards.
        /// </summary>
        /// <returns>Whether the time shard could be added</returns>
        public bool AddTimeShard()
        {
            if (timeShardCounter < 4 || AllowMoreThan4TimeShards)
            {
                timeShardCounter++;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Sets the timeshard counter to a specified amount.
        /// </summary>
        /// <param name="count">New Timeshard amount</param>
        public void SetTimeShardCounter(int count)
        {
            timeShardCounter = count;
        }

        /// <summary>
        /// Reduces the timeshard by specified amount.
        /// </summary>
        /// <param name="count">Amount to reduce the time shard count</param>
        public void ReduceTimeShardCounter(int count)
        {
            timeShardCounter -= count;
            if (timeShardCounter < 0)
                timeShardCounter = 0;
        }

        /// <summary>
        /// Whether the player can currently time travel.
        /// </summary>
        /// <returns>Whether player can timetravel.</returns>
        public bool CanTimeTravel()
        {
            return timeShardCounter >= 4 || unlimitedTimeTravel;
        }


        /// <summary>
        /// Updates all ITimetravel Entities in all room for the event timestamp placed.
        /// </summary>
        public void HandleTimeStampEnemies()
        {
            foreach (Room room in allRooms)
            {
                room?.HandleTimeStamp();
            }
        }

        /// <summary>
        /// Updates all ITimetravel Entities in all room for the event timetraveled.
        /// </summary>
        public void HandleTimeTravelEnemies()
        {
            foreach (Room room in allRooms)
            {
                room?.HandleTimeTravel();
            }
        }

        bool textBoxOpen;

        /// <summary>
        /// Whether the pause button was pressed. Enables pause menu or skips cutscenes. After the final cutscene restarts the game.
        /// </summary>
        public void PausePressed()
        {
            if (videoPlaying)
            {
                StopVideo();
                return;
            }

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

        /// <summary>
        /// Whether the textboxes should advance
        /// </summary>
        public void NextButtonPressed()
        {        
            if (!textBoxOpen)
                return;

            if (Time.realtimeSinceStartup - textBoxTime < 0.5f)
                return;

            CloseTextBox();
        }

        /// <summary>
        /// Changes Gamestate to Pause or unPause
        /// </summary>
        /// <param name="pause">Whether game should be paused.</param>
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

        /// <summary>
        /// Function responsible for displaying a textbox when the time left is < 60 seconds. 
        /// Only triggers box one time. It does not trigger when the player already timetraveled before
        /// </summary>
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

        /// <summary>
        /// Shows a textbox with specified message and pauses the game.
        /// </summary>
        /// <param name="message">Text to display inside the text box</param>
        public void ShowTextBox(string message)
        {
            textBoxTime = Time.realtimeSinceStartup;
            systemMessage.SetActive(true);
            systemMessage.GetComponentInChildren<TextMeshProUGUI>().text = message;
            Pause(true);
            textBoxOpen = true;
        }

        /// <summary>
        /// Changes sprite atlas of current textbox message. Needed for displaying the button sprites inside the text.
        /// </summary>
        /// <param name="spriteName"></param>
        public void ChangeInfoTextSprite(string spriteName)
        {
            TMP_SpriteAsset spriteAsset = Resources.Load<TMP_SpriteAsset>("Controller/" + spriteName);
            systemMessage.GetComponentInChildren<TextMeshProUGUI>().spriteAsset = spriteAsset;
        }

        /// <summary>
        /// Closes the current open textbox. Also unpauses the game.
        /// </summary>
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

        /// <summary>
        /// Changes ambient pitch according to the time left.
        /// </summary>
        /// <returns>Current pitch</returns>
        public float CalculateAmbientPitch()
        {
            float res = 1;
            if (timeLeft <= startTime * 0.9f)
                res = 1 + (startTime * 0.9f - timeLeft) / (startTime * 0.8f);
            res *= worldTimeScale;
            return res;
        }

        /// <summary>
        /// Changes game to won state. Display Victory panel with time left and triggers end cutscene.
        /// </summary>
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