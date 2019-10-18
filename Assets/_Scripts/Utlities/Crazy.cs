using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace TE
{
    /// <summary>
    /// Effect of the crazy gem to change the tilemaps color at random.
    /// </summary>
    public class Crazy : MonoBehaviour
    {
        public Tilemap[] tilemaps;

        float timer = 0;

        bool crazyModeOn = false;


        public void EnableCrazy()
        {
            crazyModeOn = true;
        }

        private void Update()
        {
            if (!crazyModeOn)
                return;
            
            timer += Time.deltaTime;
            if (timer > 0.2f)
            {
                timer = 0;
                foreach (Tilemap tilemap in tilemaps)
                {
                    tilemap.color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
                }               
            }
        }
    }
}
