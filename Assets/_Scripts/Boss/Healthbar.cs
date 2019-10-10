using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Healthbar : MonoBehaviour
{
    public Image fillBar;
    public GameObject finalBossImage;
    public GameObject shroomieImage;

    float nextFill;

    public void Activate(BossType boss)
    {
        switch (boss)
        {
            case BossType.SHROOMIE:
                finalBossImage.SetActive(false);
                shroomieImage.SetActive(true);
                break;
            case BossType.PROKRASTINATION:
                finalBossImage.SetActive(true);
                shroomieImage.SetActive(false);
                break;
            default:
                break;
        }
        gameObject.SetActive(true);
        fillBar.fillAmount = 1;
        nextFill = 1;
    }

    private void Update()
    {
        fillBar.fillAmount = Mathf.Lerp(fillBar.fillAmount, nextFill, Time.deltaTime * 4);
    }

    public void DeActivate()
    {
        gameObject.SetActive(false);   
    }


    public void UpdateBar(int curHealth, int maxHealth)
    {
        nextFill = (float) curHealth / maxHealth;
    }

    public enum BossType
    {
        SHROOMIE, PROKRASTINATION
    }
}
