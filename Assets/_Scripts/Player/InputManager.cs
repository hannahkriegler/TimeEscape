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
        private bool attack;
        private bool jumpPressed;
        private bool timeStamp;
        private bool timeTravel;
        private bool timeSkill1, timeSkill2, timeSkill3, timeSkill4;
        private bool dash;
        private bool pause;
        private Vector2 movement;

        private bool dashCheck;

        private bool jump;
        bool didJump;

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

            attack = Input.GetButtonDown("Attack");
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
                player.CombatMelee.Attack();
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
        }
    }
}