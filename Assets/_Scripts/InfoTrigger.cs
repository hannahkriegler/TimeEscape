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

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                Game.instance.ShowInfo(message, duration);
            }
        }
    }
}
