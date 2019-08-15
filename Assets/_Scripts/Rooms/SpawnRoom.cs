using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRoom : Rooms
{
    public SpawnRoom(RoomTypes roomTypesType, Enemy[] allEnemies, Enemy[] aliveEnemies, Loot[] allLoot, Loot[] collectedLoot) : base(roomTypesType, allEnemies, aliveEnemies, allLoot, collectedLoot)
    {
    }


}
