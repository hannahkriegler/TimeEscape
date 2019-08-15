using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobRoom : Rooms
{
    public MobRoom(RoomTypes roomTypesType, Enemy[] allEnemies, Enemy[] aliveEnemies, Loot[] allLoot, Loot[] collectedLoot) : base(roomTypesType, allEnemies, aliveEnemies, allLoot, collectedLoot)
    {
    }
}
