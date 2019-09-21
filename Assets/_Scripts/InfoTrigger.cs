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

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (triggerd)
                return;

            if (collision.gameObject.CompareTag("Player"))
            {
                Game.instance.ShowTextBox(message);
                triggerd = true;
            }
        }
    }
}
