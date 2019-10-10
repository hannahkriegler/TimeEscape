using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TE
{
    public class SwordPickup : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if (player != null)
            {
                player.hasSword = true;
                Destroy(gameObject);
            }
        }
    }
}
