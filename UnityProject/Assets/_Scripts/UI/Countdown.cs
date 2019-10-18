using TE;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace TE.UI
{
    public class Countdown : MonoBehaviour
    {

        public Image countdownImage;
        public TextMeshProUGUI countdownText;

        void Update()
        {
            float timeLeft = Game.instance.timeLeft;
            int min = Mathf.FloorToInt(timeLeft / 60f);
            int sec = Mathf.FloorToInt((timeLeft - min * 60));
        
            countdownText.text = min.ToString();
            countdownImage.fillAmount = Mathf.Lerp(countdownImage.fillAmount, sec / 60f, Time.deltaTime * 5);
        }

    }
}
