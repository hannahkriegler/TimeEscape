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

        public bool newMode;
        public GameObject[] splitters;

        private void FixedUpdate()
        {
            _timeShardCounter = Game.instance.timeShardCounter;
            if (newMode)
            {
                UpdateSegments(_timeShardCounter);
            }
            else
            {
                timesShard_Fill.fillAmount = _timeShardCounter / 4f;
            }
           
        }

        void UpdateSegments(int count)
        {
            for (int i = 0; i < splitters.Length; i++)
            {
                splitters[i].SetActive(count > i);
            }
        }
    }
}
