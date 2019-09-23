using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    private bool activated = false;
    public GameObject background;
    private Boss boss;
    private Image self;
    private int maxHealth = 0;

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindGameObjectWithTag("Boss").GetComponent<SpriteRenderer>().enabled == true && activated)
        {
            ActivateSelf();
            boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss>();
            maxHealth = boss.hitPoints;
        }

        self.fillAmount = boss.hitPoints / maxHealth;
    }

    private void ActivateSelf()
    {
        activated = true;
        background.SetActive(true);
        gameObject.GetComponent<Image>().enabled = true;
    }
}
