using System;
using TE;
using UnityEngine;
using UnityEngine.UI;

namespace TE.UI
{
    public class TimeShard : MonoBehaviour
    {
        public Image timesShard_Fill;
        private int _timeShardCounter;
        
        private void FixedUpdate()
        {
            _timeShardCounter = Game.instance.timeShardCounter;
            timesShard_Fill.fillAmount = _timeShardCounter / 4f;
        }
    }
}
