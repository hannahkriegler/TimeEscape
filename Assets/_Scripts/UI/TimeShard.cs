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

        public int layer = 0;
        public bool newMode;
        public GameObject[] splitters;

        public GameObject[] components;
        private void FixedUpdate()
        {
            _timeShardCounter = Game.instance.timeShardCounter;
            if (newMode)
            {
                UpdateSegments(_timeShardCounter);
            }
            else
            {
                if (layer == 0)
                {
                    timesShard_Fill.fillAmount = _timeShardCounter / 4f;
                    Hide(false);
                }
                else
                {
                    float counter = _timeShardCounter - 4 * layer;
                    if (counter <= 0)
                        Hide();
                    else
                    {
                        if (counter > 4.0f)
                            counter = 4.0f;
                        timesShard_Fill.fillAmount = counter / 4f;
                        Hide(false);
                    }
                }
            }
           
        }

        void UpdateSegments(int count)
        {
            for (int i = 0; i < splitters.Length; i++)
            {
                splitters[i].SetActive(count > i);
            }
        }

        void Hide(bool hide = true)
        {
            foreach (GameObject g in components)
            {
                g.SetActive(!hide);
            }
        }
    }
}
