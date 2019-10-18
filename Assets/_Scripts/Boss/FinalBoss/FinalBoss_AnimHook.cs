using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TE
{
    public class FinalBoss_AnimHook : MonoBehaviour
    {
        FinalBoss boss;
        public void Init(FinalBoss boss)
        {
            this.boss = boss;
        }

        /// <summary>
        /// Animation Function Trigger to enable the damage collision check
        /// </summary>
        public void EnableMeleeDamageCollider()
        {
            boss.sword.AllowHit(true);
        }


        /// <summary>
        /// Animation Function Trigger to disable the damage collision check
        /// </summary>
        public void DisableMeleeDamageCollider()
        {
            boss.sword.AllowHit(false);
        }

        /// <summary>
        /// Animation Function Trigger to unleash the fireball
        /// </summary>
        public void CastFireBall()
        {
            boss.TriggerFireBall();
        }
    }
}
