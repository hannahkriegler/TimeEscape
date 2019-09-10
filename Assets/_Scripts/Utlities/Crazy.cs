using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace TE
{
    public class Crazy : MonoBehaviour
    {
        public Tilemap tilemap;

        float timer = 0;

        private void Update()
        {
            timer += Time.deltaTime;
            if (timer > 0.2f)
            {
                timer = 0;
                tilemap.color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
            }
        }
    }
}
