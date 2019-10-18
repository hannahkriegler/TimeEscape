using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TE
{
    /// <summary>
    /// Passes game states to the shaders paramters
    /// </summary>
    public class BackgroundHook : MonoBehaviour
    {
        Material mat;
        float curTime;

        private void Start()
        {
            mat = GetComponent<Renderer>().material;
            mat.SetVector("_Tiling", transform.lossyScale);
        }
        private void Update()
        {
            float time = Game.instance.startTime - Game.instance.timeLeft;
            //time = Mathf.Clamp(time, 0, Game.instance.startTime);
            //Lerps timechange for smoother transitions
            curTime = Mathf.Lerp(curTime, time, Time.deltaTime * 5);
            mat.SetFloat("_CurTime", curTime);
        }
    }
}
