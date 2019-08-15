using System;
using System.Collections;
using System.Collections.Generic;
using TE;
using UnityEngine;

public class Loot : MonoBehaviour
{
    public enum LootTypes
    {
        Time,
        Zeitsplitter
    }

    public int addedTime = 10;
    public LootTypes lootType;

    public void OnTriggerEnter2D(Collider2D player)
    {
        if (!player.CompareTag("Player")) return;

        PickUpLoot();
        HideSprite();
    }

    public virtual void PickUpLoot()
    {
        switch (lootType)
        {
            case LootTypes.Time:
                Debug.Log("Increased Time");
                Game.IncreaseTime(addedTime);
                break;
            case LootTypes.Zeitsplitter:
                Debug.Log("Picked Up Zeitsplitter");
                // TODO need to implement this
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
    }

    public void HideSprite()
    {
        this.GetComponent<SpriteRenderer>().enabled = false;
        this.GetComponent<CircleCollider2D>().enabled = false;
    }
    
    
}
