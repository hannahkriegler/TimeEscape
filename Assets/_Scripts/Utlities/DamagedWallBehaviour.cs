using System;
using System.Collections;
using System.Collections.Generic;
using TE;
using UnityEngine;

public class DamagedWallBehaviour : MonoBehaviour, IHit, ITimeTravel
{
    public bool shake;
    public float timeToShake = 2f;
    private float currentTime;
    private bool isShaking = false;

    private bool saveDestroy = false;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("FireBall")) return;
        DestroyWall();
        return;


    }

    private void DestroyWall()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        
        if (shake)
        {
            currentTime = timeToShake;
            StartCoroutine(MoveWallPart());
            shake = false;
            isShaking = true;
        }

        if (isShaking)
        {
            currentTime -= Time.deltaTime;
        }
    }


    IEnumerator MoveWallPart()
    {
        while (currentTime > 0)
        {
            yield return new WaitForEndOfFrame();
            Debug.Log("shake");
            foreach (Transform wall in transform)
            {
                wall.transform.localRotation = new Quaternion(wall.transform.localRotation.x, 
                    wall.transform.localRotation.y, Mathf.Sin(Time.time * 20f) * .05f, 1);
            }
        }
        
        foreach (Transform wall in transform)
        {
            wall.transform.rotation = Quaternion.Lerp(wall.transform.rotation, new Quaternion(0,0,0,1), 10f );
        }

        isShaking = false;

        
    }


    public void OnHit(int damage, GameObject attacker, bool knockBack = true)
    {

        shake = true;
    }

    public void HandleTimeStamp()
    {
        saveDestroy = gameObject.activeSelf;
    }

    public void HandleTimeTravel()
    {
        gameObject.SetActive(saveDestroy);
    }
}
