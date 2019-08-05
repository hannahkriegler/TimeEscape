using System;
using UnityEngine;

namespace TE
{
    public class InputManager : MonoBehaviour
    {
        private Game game;
        
        [Header("References")]
        public Player player;
        
        //Inputs
        private bool active_skill;
        private bool standard_attack;
        private bool jump;
        private bool timeStamp;
        private bool timeSkill1, timeSkill2, timeSkill3, timeSkill4;
        private bool dash;
        private bool pause;
        private Vector2 movement;

        public void Init(Game game)
        {
            this.game = game;
            player.Init(game);
        }
        
        private void Update()
        {
            
        }
    }
}