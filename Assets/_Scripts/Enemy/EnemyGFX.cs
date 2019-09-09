using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

namespace TE
{
    public class EnemyGFX : MonoBehaviour
    {
        public AIPath aiPath;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (aiPath.desiredVelocity.x <= 0.01)
            {
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if (aiPath.desiredVelocity.x >= -0.01f)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
        }
    }
}
