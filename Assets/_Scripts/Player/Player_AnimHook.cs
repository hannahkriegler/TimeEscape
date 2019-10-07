using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TE
{
    public class Player_AnimHook : MonoBehaviour
    {
        Player player;
        public void Init(Player player)
        {
            this.player = player;
        }

        public void EnableMeleeDamageCollider()
        {
            player.sword.AllowHit(true);
        }

        public void DisableMeleeDamageCollider()
        {
            player.sword.AllowHit(false);
        }
    }
}