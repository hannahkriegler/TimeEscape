using System;
using UnityEngine;

namespace TE
{
    /// <summary>
    /// Responsible for each Input Command the Player can execute. Updates the Player Class according to the input.
    /// </summary>
    public class InputManager : MonoBehaviour
    {
        private Game game;

        [Header("References")] public Player player;

        //Inputs
        private bool active_skill;
        private bool attackPressed;
        private bool jumpPressed;
        private bool timeStamp;
        private bool timeTravel;
        private bool timeSkill_slow, timeSkill_fast;
        private bool dash;
        private bool pause;
        private Vector2 movement;

        private bool skillCheck;
        private bool dashCheck;

        private bool jump;
        bool didJump;
        bool attack;
        bool didAttack;

        float timeStampTimer;
        bool didPlaceTimeStamp;
        float timeTravelTimer;
        bool didTimeTravel;
        const float longPressDuration = 0.8f;

        bool timeStampAnimActive;
        bool timeTravelAnimActive;

        public void Init(Game game)
        {
            this.game = game;
            player.Init(game);
            dashCheck = false;
        }

        private void Update()
        {
            UpdateInputs();
            UpdatePlayer();
        }

        /// <summary>
        /// Updates states for all input actions.
        /// </summary>
        void UpdateInputs()
        {
            movement.x = Input.GetAxis("Horizontal");
            movement.y = Input.GetAxis("Vertical");

            pause = Input.GetButtonDown("Pause");


            //Handle Attack
            attackPressed = Input.GetButton("Attack");
            //Input Buffer
            if (attackPressed)
            {
                if (!didAttack)
                {
                    attack = true;
                }
            }
            else
            {
                didAttack = false;
            }

            //Active Skill
            bool controllerSkill = Input.GetAxis("Skill") > 0.5f;
            bool keySkill = Input.GetButtonDown("Skill");
            active_skill = controllerSkill || keySkill;

            if (skillCheck)
            {
                if (!controllerSkill && !keySkill)
                    skillCheck = false;
            }

            //Jump Handling
            jumpPressed = Input.GetButton("Jump");
            if (jumpPressed)
            {
                if (!didJump)
                {
                    jump = true;
                }
            }
            else
            {
                didJump = false;
            }

            //Handles Timestamps and the ButtonPrompt above the Character
            bool timeStampPressed = Input.GetButton("TimeStamp");
            timeStamp = false;
            if (timeStampPressed && game.session.canPlaceTimeStamp && player.Movement.grounded)
            {
                if (!didPlaceTimeStamp)
                {
                    if (!timeStampAnimActive)
                    {
                        player.animator.Play("Cast_Time");
                        timeStampAnimActive = true;
                    }

                    player.buttomPrompt.Init(ButtonType.B);
                    timeStampTimer += Time.deltaTime;
                    player.buttomPrompt.SetFillAmount(timeStampTimer / longPressDuration, !game.CanTimeTravel());
                    if (timeStampTimer > longPressDuration)
                    {
                        didPlaceTimeStamp = true;
                        timeStamp = true;
                        timeStampAnimActive = false;
                        player.buttomPrompt.Disable();
                    }
                }
            }
            else
            {
                didPlaceTimeStamp = false;
                if (timeStampAnimActive)
                {
                    player.animator.Play("Default");
                    timeStampAnimActive = false;
                }
                timeStampTimer = 0;
            }

            //Handles Timetravels and the ButtonPrompt above the Character
            timeTravel = false;
            bool timeTravelPressed = Input.GetButton("TimeTravel");
            bool timeTravelJustPressed = Input.GetButtonDown("TimeTravel");

            if (timeTravelPressed && game.session.canTimeTravel && game.CanTimeTravel())
            {
                if (!didTimeTravel)
                {
                    if (!timeTravelAnimActive)
                    {
                        player.animator.Play("Cast_Time");
                        timeTravelAnimActive = true;
                    }

                    player.buttomPrompt.Init(ButtonType.Y);
                    timeTravelTimer += Time.deltaTime;
                    player.buttomPrompt.SetFillAmount(timeTravelTimer / longPressDuration);
                    if (timeTravelTimer > longPressDuration)
                    {
                        didTimeTravel = true;
                        timeTravel = true;
                        player.buttomPrompt.Disable();
                    }
                }
            }
            else
            {
                didTimeTravel = false;
                if (timeTravelAnimActive)
                {
                    player.animator.Play("Default");
                    timeTravelAnimActive = false;
                }
                timeTravelTimer = 0;
             
            }

            //Resets buttom Prompts
            if (timeTravelJustPressed && game.session.canTimeTravel && !game.CanTimeTravel())
                player.buttomPrompt.ShowTimeTravelDisabled();

            if (!timeTravelPressed && !timeStampPressed)
                player.buttomPrompt.Disable();

            //The Controllers Trigger is not a Button its an Axis.
            bool controllerDash = Input.GetAxis("Dash") > 0.5f;
            bool keyDash = Input.GetButtonDown("Dash");
            dash = controllerDash || keyDash;

            if (dashCheck)
            {
                if (!controllerDash && !keyDash)
                    dashCheck = false;
            }

            //TODO TimeSkills
            timeSkill_slow = Input.GetButton("SlowDown");
            timeSkill_fast = Input.GetButton("SpeedUp");

            //Both Skills counter itselfs
            if (timeSkill_slow && timeSkill_fast)
            {
                timeSkill_slow = false;
                timeSkill_fast = false;
            }

            //Next
            bool nextButtonPressed = Input.GetButtonDown("Jump");
            if (nextButtonPressed)
            {
                game.NextButtonPressed();
            }
        }

        public bool SomethingWasPressed()
        {
            return attackPressed || active_skill || jumpPressed || timeStamp || timeTravel || dash || timeSkill_slow || timeSkill_fast;
        }

        /// <summary>
        /// Executes functions on the Player class according to the inputs.
        /// </summary>
        void UpdatePlayer()
        {
            if (player.dead)
                return;

            //Pause
            if (pause)
            {
                game.PausePressed();
            }
            if (game.gameIsPaused)
            {
                //Fix Jump Issue
                didJump = true;
                jump = false;
                return;
            }

            //Movement
            player.Movement.Tick();
            player.Movement.Move(movement);
            player.Movement.higherJump = jumpPressed;

            if (jump)
            {
                bool jumped = player.Movement.Jump();
                if (jumped)
                {
                    didJump = true;
                    jump = false;
                }
            }


            if (dash && !dashCheck)
            {
                dash = false;
                player.Movement.Dash();
                dashCheck = true;
            }

            //Attack Handling
            if (attack)
            {
                bool attacked = player.CombatMelee.Attack();
                if (attacked)
                {
                    didAttack = true;
                    attack = false;
                }
            }

            //Skills
            if (active_skill && !skillCheck)
            {
                active_skill = false;
                player.CombatSkill.ActivateActiveSkill();
                skillCheck = true;
            }

            //Time Skills
            if (timeStamp)
            {
                player.TimeSkills.PlaceTimestamp();
            }
            else
            if (timeTravel)
            {
                player.TimeSkills.TimeTravel();
            }

            if (timeSkill_fast)
            {
                player.TimeSkills.SpeedUpTime();
            }
            else
            if (timeSkill_slow)
            {
                player.TimeSkills.SlowDownTime();
            }
            else
            {
                player.TimeSkills.NormalTime();
            }
        }
    }
}