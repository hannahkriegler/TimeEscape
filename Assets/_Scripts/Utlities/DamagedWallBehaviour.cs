using System;
using System.Collections;
using System.Collections.Generic;
using TE;
using UnityEngine;

public class DamagedWallBehaviour : MonoBehaviour
{
    public bool shake;
    public float timeToShake = 2f;
    private float currentTime;
    private void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag("FireBall")) return;
    }

    private void DestroyWall()
    {
        
    }

    private void Update()
    {
        if (shake)
        {
            StartCoroutine(MoveWallPart());
            shake = false;
        }
    }


    private IEnumerator MoveWallPart()
    {
        currentTime = timeToShake;
        while (currentTime > 0)
        {
            foreach (Transform wall in transform)
            {
                Debug.Log(wall.name);
                wall.transform.localRotation = new Quaternion(wall.transform.localRotation.x,
                    wall.transform.localRotation.y, Mathf.Sin(Time.time * 10f) * .07f, 1);
            }

            currentTime -= Time.deltaTime;
        }

        yield return null;
    }
    
    
}
