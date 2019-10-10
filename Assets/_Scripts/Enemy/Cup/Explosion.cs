using System;
using System.Collections;
using System.Collections.Generic;
using TE;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public int damageAmount;

    private void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            IHit hit = other.gameObject.GetComponent<IHit>();
            hit.OnHit(damageAmount, gameObject);
            
        }
    }

   

   
}
