using System;
using UnityEngine;

namespace TE
{
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

        private bool dashCheck;

        private bool jump;
        bool didJump;
        bool attack;
        bool didAttack;

        public void Init(Game game)
        {
            this.game = game;
            player.Init(game);
            dashCheck = false;
        }

        private void FixedUpdate()
        {
            UpdateInputs();
            UpdatePlayer();
        }

        void UpdateInputs()
        {
            movement.x = Input.GetAxis("Horizontal");
            movement.y = Input.GetAxis("Vertical");

            //Handle Attack
            attackPressed = Input.GetButton("Attack");
            //Input Buffer
            if(attackPressed)
            {
                if(!didAttack)
                {
                    attack = true;
                }
            }
            else
            {
                didAttack = false;
            }


            active_skill = Input.GetButtonDown("Skill");

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

            timeStamp = Input.GetButtonDown("TimeStamp");
            timeTravel = Input.GetButtonDown("TimeTravel");
            bool controllerDash = Input.GetAxis("Dash") > 0.5f;
            bool keyDash = Input.GetButtonDown("Dash");
            dash = controllerDash || keyDash;

            if (dashCheck)
            {
                if (!controllerDash && !keyDash)
                    dashCheck = false;
            }

            //TODO TimeSkills
            timeSkill_slow = Input.GetKey(KeyCode.Z);
            timeSkill_fast = Input.GetKey(KeyCode.U);

            //Both Skills counter itselfs
            if(timeSkill_slow && timeSkill_fast)
            {
                timeSkill_slow = false;
                timeSkill_fast = false;
            }
        }

        void UpdatePlayer()
        {
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
                if(attacked)
                {
                    didAttack = true;
                    attack = false;
                }
            }

            //Skills
            if (active_skill)
            {
                active_skill = false;
                player.CombatSkill.ActivateActiveSkill();
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