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
        Zeitsplitter,
        Gem
    }

    public enum SpawnTypes
    {
        Spawn,
        NotSpawn,
        ReSpawn
    }

    public int addedTime = 10;
    public LootTypes lootType;

    public SpawnTypes spawnType = SpawnTypes.Spawn;

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
                if (Game.portalIsSet) spawnType = SpawnTypes.ReSpawn;
                break;
            case LootTypes.Zeitsplitter:
                Debug.Log("Picked Up Zeitsplitter");
                if (Game.portalIsSet) spawnType = SpawnTypes.ReSpawn;
                Game.AddZeitsplitter();
                break;
            case LootTypes.Gem:
                if (Game.portalIsSet) spawnType = SpawnTypes.NotSpawn;
                GemBehaviour();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
    }

    public void HideSprite()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<CircleCollider2D>().enabled = false;
    }

    public void ShowSprite()
    {
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<CircleCollider2D>().enabled = true;
    }

    public virtual void GemBehaviour()
    {
        Debug.Log("Picked Up Gem");
    }
    
    
}
