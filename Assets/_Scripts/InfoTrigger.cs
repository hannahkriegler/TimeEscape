using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TE
{
    public class InfoTrigger : MonoBehaviour
    {
        [TextArea]
        public string message;

        bool triggerd;

        public bool unlockTimeTravel;
        public bool unlockTimeStamp;
        public bool specialTimeStampBlock;

        bool waitForTrigger = false;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (triggerd)
                return;

            if (collision.gameObject.CompareTag("Player"))
            {
                Game.instance.ShowTextBox(message);
                if (specialTimeStampBlock)
                {
                    waitForTrigger = true;
                    Game.instance.player.Movement.KnockBack(200, transform);
                }

                if(!waitForTrigger)
                 triggerd = true;
                if (unlockTimeTravel)
                    Game.instance.session.UnlockTimeTravel();
                if (unlockTimeStamp)
                    Game.instance.session.UnlockTimestamp();
            }
        }

        private void Update()
        {
            if (waitForTrigger)
            {
                if (specialTimeStampBlock)
                {
                    if (Game.instance.player.TimeSkills.firstTimeStamp)
                    {
                        triggerd = true;
                    }
                }
            }
        }
    }
}
