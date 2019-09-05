using TE;
using UnityEngine;
using UnityEngine.UI;

namespace TE.UI
{
    public class Countdown : MonoBehaviour
    {

        public Image countdownImage;
        public Text countdownText;

        void Update()
        {
            float timeLeft = Game.timeLeft;
            int min = Mathf.FloorToInt(timeLeft / 60f);
            int sec = Mathf.FloorToInt((timeLeft - min * 60));
        
            countdownText.text = min.ToString();
            countdownImage.fillAmount = sec / 60f;
        }

    }
}
