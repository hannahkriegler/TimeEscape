using UnityEngine;
using System.Collections;

namespace TE
{
    public interface IHit
    {
        void OnHit(int damage);
    }
}