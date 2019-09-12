using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TE
{
    public class InfoTrigger : MonoBehaviour
    {
        [TextArea]
        public string message;
        public float duration = 3.0f;

        bool triggerd;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (triggerd)
                return;

            if (collision.gameObject.CompareTag("Player"))
            {
                Game.instance.ShowInfo(message, duration);
                triggerd = true;
            }
        }
    }
}
