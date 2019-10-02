using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Healthbar : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public Image fillBar;

    float nextFill;

    public void Activate(string bossName)
    {
        nameText.text = bossName;
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
}
