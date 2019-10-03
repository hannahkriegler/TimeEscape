using UnityEngine;

namespace TE
{
    public class FireBallSkill : Skill
    {
        public override void Activate(Player player)
        {
            GameObject fireBall = Object.Instantiate(player.fireBall);
            fireBall.transform.position = player.transform.position + player.transform.forward * 1.2f;
            fireBall.GetComponent<FireBall>().Shoot(2, player.Movement.GetForwardDir());
            Game.instance.DecreaseTime(20);
        }
    }
}