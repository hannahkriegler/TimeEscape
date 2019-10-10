using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TE
{
    public class ButtomPrompt : MonoBehaviour
    {
        public GameObject portalSymbol;
        public GameObject timeTravel;


        public Image circleImage;
        public Color baseColor;


        public void Init(ButtonType buttonType)
        {
            gameObject.SetActive(true);
            switch (buttonType)
            {
                case ButtonType.B:
                    portalSymbol.SetActive(true);
                    timeTravel.SetActive(false);
                    break;
                case ButtonType.Y:
                    portalSymbol.SetActive(false);
                    timeTravel.SetActive(true);
                    break;
                default:
                    break;
            }
        }

        public void SetFillAmount(float fill, bool changeColor = false)
        {
            if (changeColor)
                circleImage.color = Color.red;
            else
              circleImage.color = baseColor;
            circleImage.fillAmount = fill;
        }

        public void ShowTimeTravelDisabled()
        {
            if (!rdy)
                return;
            portalSymbol.SetActive(false);
            timeTravel.SetActive(true);
            gameObject.SetActive(true);
            circleImage.color = Color.red;
            circleImage.fillAmount = 1;
            rdy = false;
            StartCoroutine(Hide());
        }

        bool rdy = true;
        IEnumerator Hide()
        {
            yield return new WaitForSeconds(0.4f);
            gameObject.SetActive(false);
            rdy = true;
        }


        public void Disable()
        {
            if (!rdy)
                return;

            gameObject.SetActive(false);
        }
    }

    public enum ButtonType
    {
        B, Y
    }
}
