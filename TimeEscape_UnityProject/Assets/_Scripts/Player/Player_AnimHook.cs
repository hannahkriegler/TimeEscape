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

        /// <summary>
        /// Animation Function Trigger to enable the damage collision check
        /// </summary>
        public void EnableMeleeDamageCollider()
        {
            player.sword.AllowHit(true);
        }

        /// <summary>
        /// Animation Function Trigger to disable the damage collision check
        /// </summary>
        public void DisableMeleeDamageCollider()
        {
            player.sword.AllowHit(false);
        }

        /// <summary>
        /// Animation Function Trigger to unleash the fireball
        /// </summary>
        public void CastFireBall()
        {
            player.CombatSkill.TriggerSkill();
        }
    }
}