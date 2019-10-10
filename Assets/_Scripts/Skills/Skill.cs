using UnityEngine;

namespace TE
{
    public abstract class Skill
    {
        public abstract void Activate(Player player);

        public abstract void Trigger(Player player);
    }
}