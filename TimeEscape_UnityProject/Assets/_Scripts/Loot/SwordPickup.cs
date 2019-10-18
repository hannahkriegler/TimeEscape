using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace TE
{
    /// <summary>
    /// When the player runs over the pickup the player can use the sword. After picking up displays a textbox to explain attacking.
    /// </summary>
    public class SwordPickup : MonoBehaviour
    {
        [TextArea]
        public string message;

        public TMP_SpriteAsset spriteAsset;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if (player != null)
            {
                player.hasSword = true;
                Game.instance.systemMessage.GetComponentInChildren<TextMeshProUGUI>().spriteAsset = spriteAsset;
                Game.instance.ShowTextBox(message);
                Destroy(gameObject);
            }
        }
    }
}
