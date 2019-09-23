using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TE
{
    public class ButtomPrompt : MonoBehaviour
    {
        public Sprite y_button;
        public Sprite b_button;

        public Image buttonImage;
        public Image circleImage;


        public void Init(ButtonType buttonType)
        {
            gameObject.SetActive(true);
            switch (buttonType)
            {
                case ButtonType.B:
                    buttonImage.sprite = b_button;
                    break;
                case ButtonType.Y:
                    buttonImage.sprite = y_button;
                    break;
                default:
                    break;
            }
        }

        public void SetFillAmount(float fill)
        {
            circleImage.fillAmount = fill;
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }
    }

    public enum ButtonType
    {
        B, Y
    }
}
