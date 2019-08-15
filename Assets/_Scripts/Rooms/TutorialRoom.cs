using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialRoom : Rooms
{
    public TutorialRoom(RoomTypes roomTypesType, Enemy[] allEnemies, Enemy[] aliveEnemies, Loot[] allLoot, Loot[] collectedLoot) : base(roomTypesType, allEnemies, aliveEnemies, allLoot, collectedLoot)
    {
    }
    
    

    private Rooms _room;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void RoomBehaviour()
    {
        Debug.Log("Welocme to " + roomTypesType.ToString() + " Room!");
    }
    
}
