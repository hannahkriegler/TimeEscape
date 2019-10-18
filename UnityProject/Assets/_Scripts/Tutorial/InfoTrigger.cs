using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace TE
{
    public class InfoTrigger : MonoBehaviour
    {
        [TextArea]
        public string message;

        public TMP_SpriteAsset spriteAsset;

        bool triggerd;

        public bool unlockTimeTravel;
        public bool unlockTimeStamp;
        public bool specialTimeStampBlock;

        [Tooltip("Whether the trigger should stay active after displaying the textbox.")]
        bool waitForTrigger = false;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (Game.instance.skipTutorials)
                return;
            
            if (triggerd)
                return;

            if (collision.gameObject.CompareTag("Player"))
            {
                Game.instance.systemMessage.GetComponentInChildren<TextMeshProUGUI>().spriteAsset = spriteAsset;
                Game.instance.ShowTextBox(message);
                if (specialTimeStampBlock)
                {
                    waitForTrigger = true;         
                }

                if(!waitForTrigger)
                 triggerd = true;

                //Unlocks the time skill explained in the tutorial box. 
                if (unlockTimeTravel)
                    Game.instance.session.UnlockTimeTravel();
                if (unlockTimeStamp)
                    Game.instance.session.UnlockTimestamp();
            }
        }

        private void Update()
        {
            if (specialTimeStampBlock)
            {
                //Knockbacks player if he did not place a timestamp
                if (waitForTrigger && !Game.instance.IsTextBoxOpen())
                {
                    Game.instance.player.Movement.KnockBack(800, transform);
                    waitForTrigger = false;
                }

                //Disables knockback when player placed his first timestamp
                if (Game.instance.player.TimeSkills.firstTimeStamp)
                {
                    triggerd = true;
                }
            }
        }   
        
    }
}
