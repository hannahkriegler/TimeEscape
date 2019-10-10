using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace TE
{
    public class LooseTime : MonoBehaviour
    {
        public TextMeshProUGUI text;
        public Vector2 startPos;

        public void TimeChange(int time)
        {
            gameObject.SetActive(true);
            transform.localPosition = startPos;
            string content = time.ToString(); 
            if(time < 0)
            {
                text.color = Color.red;
                text.text = content;
            }
            else
            {
                text.color = Color.green;
                text.text = "+" + content;
            }

        }

        private void Update()
        {
            transform.localPosition += Vector3.up * 2.0f * Time.deltaTime;
            if (transform.localPosition.y > startPos.y + 3.0f)
                gameObject.SetActive(false);
        }
    }
}
