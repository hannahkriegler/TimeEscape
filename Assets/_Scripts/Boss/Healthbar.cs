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

    /// <summary>
    /// Shows the healthbar. Changes icon according to the current boss.
    /// </summary>
    /// <param name="boss">Which bosses healthbar to display</param>
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

    /// <summary>
    /// Hides Healthbar
    /// </summary>

    public void DeActivate()
    {
        gameObject.SetActive(false);   
    }

    /// <summary>
    /// Changes progress status of the healthbar to curHealth/maxHealth
    /// </summary>
    /// <param name="curHealth">Currenthealth of the boss</param>
    /// <param name="maxHealth">Maxhealth of the boss</param>
    public void UpdateBar(int curHealth, int maxHealth)
    {
        nextFill = (float) curHealth / maxHealth;
    }

    public enum BossType
    {
        SHROOMIE, PROKRASTINATION
    }
}
