using System.Collections;
using System.Collections.Generic;
using TE;
using UnityEngine;

public class Boss_Minion : Enemy
{
    private Transform target;

    // Start is called before the first frame update
    void Start()
    {
        target = Game.instance.player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        //MoveToPlayer();
    }

    void MoveToPlayer()
    {
        //rotate to look at the player
        transform.LookAt(target.position);
        transform.Rotate(new Vector3(0, -90, 0), Space.Self); //correcting the original rotation


        //move towards the player
        if (Vector3.Distance(transform.position, target.position) > 1f)
        {
            //move if distance from target is greater than 1
            transform.Translate(new Vector3(3 * Time.deltaTime, 0, 0));
        }
    }
}
