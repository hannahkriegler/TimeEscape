using UnityEngine;
using System.Collections;

namespace TE
{
    /// <summary>
    /// Units need to implement this interface to take damage.
    /// </summary>
    public interface IHit
    {
        void OnHit(int damage, GameObject attacker, bool knockBack = true);
    }
}