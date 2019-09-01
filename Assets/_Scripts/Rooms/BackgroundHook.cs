using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TE
{
    public class BackgroundHook : MonoBehaviour
    {
        Material mat;
        private void Start()
        {
            mat = GetComponent<Renderer>().material;
        }
        private void Update()
        {
            float time = Game.instance.startTime - Game.TimeLeft;
            time = Mathf.Clamp(time, 0, Game.instance.startTime);
            mat.SetFloat("_CurTime", time);
        }
    }
}
