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

        public void EnableMeleeDamageCollider()
        {
            boss.sword.AllowHit(true);
        }

        public void DisableMeleeDamageCollider()
        {
            boss.sword.AllowHit(false);
        }

        public void CastFireBall()
        {
            boss.TriggerFireBall();
        }
    }
}
