using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class RoomBehaviour : MonoBehaviour
{
    public enum Room
    {
        Spawn,
        Tutorial,
        Loot,
        Mob,
        Normal,
        Boss
    };

    public Room roomType;

    public Enemy[] AllEnemies;
    [HideInInspector]
    public Enemy[] AliveEnemies;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D player)
    {
        if (player.CompareTag("Player"))
        {
            switch (roomType)
            {
                case Room.Spawn:
                    SpawnRoomBehaviour();
                    break;
                case Room.Tutorial:
                    TutorialRoomBehaviour();
                    break;
            }
        }
    }

    private void SpawnRoomBehaviour()
    {
        Debug.Log("Welcome In the Spawn Room!");
    }

    private void TutorialRoomBehaviour()
    {
        Debug.Log("Welcome in the Tutorial Room");
    }
}
