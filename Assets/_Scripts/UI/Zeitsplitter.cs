using System;
using TE;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI
{
    public class Zeitsplitter : MonoBehaviour
    {
        public Image zeitsplitterCounterImage;
        private int _zeitsplitterCounter;
        
        private void FixedUpdate()
        {
            _zeitsplitterCounter = Game.ZeitsplitterCounter;
            zeitsplitterCounterImage.fillAmount = _zeitsplitterCounter / 4f;
        }
    }
}
