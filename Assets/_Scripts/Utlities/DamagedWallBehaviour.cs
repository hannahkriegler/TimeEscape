using System;
using System.Collections;
using System.Collections.Generic;
using TE;
using UnityEngine;
/// <summary>
/// The damaged wall dissapears if it gets hit by the fireball, else it will shake
/// </summary>
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

    /// <summary>
    /// A coroutine to shake all wall parts if the player hits the wall with the sword
    /// </summary>
    /// <returns></returns>
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
