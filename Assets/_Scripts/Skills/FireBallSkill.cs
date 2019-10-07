using UnityEngine;

namespace TE
{
    public class FireBallSkill : Skill
    {
        public override void Activate(Player player)
        {
            GameObject fireBall = Object.Instantiate(player.fireBall);
            Vector2 dir = player.Movement.GetForwardDir();
            fireBall.transform.position = player.transform.position + (Vector3) dir * 1.2f + player.transform.up * 0.5f;
            fireBall.GetComponent<FireBall>().Shoot(2, dir);
            Game.instance.DecreaseTime(20);
        }
    }
}