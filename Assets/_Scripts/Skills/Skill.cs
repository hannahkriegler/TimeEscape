using UnityEngine;

namespace TE
{
    public abstract class Skill
    {
        /// <summary>
        /// Preperation for executing the active skill. (e.g. cast animation)
        /// </summary>
        /// <param name="player">Owner of the skill.</param>
        public abstract void Activate(Player player);

        /// <summary>
        /// Actualy executes the effect of the active skill. (e.g. spawning the fireball projectile)
        /// </summary>
        /// <param name="player"></param>
        public abstract void Trigger(Player player);
    }
}