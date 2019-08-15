using System.Collections;
using System.Collections.Generic;
using TE;
using UnityEngine;
using UnityEngine.UI;

public class Countdown : MonoBehaviour
{

    public Image countdownImage;
    public Text countdownText;
    
    private static float _timeLeft ;

    private Game _game;
    
    // Start is called before the first frame update
    void Start()
    {
        _timeLeft = 300;
    }

    public void Init(Game game)
    {
        this._game = game;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _timeLeft -= Time.deltaTime * _game.coundwodnTimeScale;
        int min = Mathf.FloorToInt(_timeLeft / 60f);
        int sec = Mathf.FloorToInt((_timeLeft - min * 60));
        
        countdownText.text = min.ToString();
        countdownImage.fillAmount = sec / 60f;

        if (_timeLeft <= 0)
            _game.GameOver();
    }

    public static void IncreaseTime(float time)
    {
        _timeLeft += time;
    }

    public static void DecreaseTime(float time)
    {
        _timeLeft -= time;
    }

}
