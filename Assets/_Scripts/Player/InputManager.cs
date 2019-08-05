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
        private bool attack_up;
        private bool jump;
        private bool timeStamp;
        private bool timeSkill1, timeSkill2, timeSkill3, timeSkill4;
        private bool dash;
        private bool pause;
        private Vector2 movement;

        private bool dashCheck;
        private float chargeTimer;

        public void Init(Game game)
        {
            this.game = game;
            player.Init(game);
            chargeTimer = 0;
            dashCheck = false;
        }

        private void Update()
        {
            UpdateInputs();
            UpdatePlayer();
        }

        void UpdateInputs()
        {
            movement.x = Input.GetAxis("Horizontal");
            movement.y = Input.GetAxis("Vertical");

            attack = Input.GetButton("Attack");
            attack_up = Input.GetButtonUp("Attack");
            active_skill = Input.GetButtonDown("Skill");
            jump = Input.GetButtonDown("Jump");
            timeStamp = Input.GetButtonDown("TimeStamp");

            bool controllerDash = Input.GetAxis("Dash") > 0.5f;
            bool keyDash = Input.GetButtonDown("Dash");
            dash = controllerDash || keyDash;

            if (dashCheck)
            {
                if (!controllerDash && !keyDash)
                    dashCheck = false;
            }

            //TODO Timeskills
        }

        void UpdatePlayer()
        {
            //Movement
            player.Movement.Move(movement);

            if (jump)
            {
                jump = false;
                player.Movement.Jump();
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
                chargeTimer += player.delta;
            }

            if (attack_up)
            {
                if (chargeTimer < 0.5f)
                {
                    player.CombatMelee.Attack();
                }
                else
                {
                    player.CombatSkill.ActivateChargeSkill();
                }

                chargeTimer = 0;
            }

            //Skills
            if (active_skill)
            {
                active_skill = false;
                player.CombatSkill.ActivateActiveSkill();
            }
        }
    }
}