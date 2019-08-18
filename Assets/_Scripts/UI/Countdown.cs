using TE;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI
{
    public class Countdown : MonoBehaviour
    {

        public Image countdownImage;
        public Text countdownText;
        private static float _timeLeft ;

        // Update is called once per frame
        void FixedUpdate()
        {
            _timeLeft = Game.TimeLeft;
            int min = Mathf.FloorToInt(_timeLeft / 60f);
            int sec = Mathf.FloorToInt((_timeLeft - min * 60));
        
            countdownText.text = min.ToString();
            countdownImage.fillAmount = sec / 60f;
        }

    }
}
