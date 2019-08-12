using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
using Pathfinding;
using TE;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Seeker))]
public class EnemyAI : MonoBehaviour
{
    // What to chase?
    public Transform target;

    // How many times each second we will update our path
    public float updateRate = 2f;

    // chaching
    private Seeker seeker;
    private Rigidbody2D rb;
    
    // the calculated path
    public Path path;
    
    // the AI's speed per second
    public float speed = 200f;
    //public ForceMode2D fMode;

    [FormerlySerializedAs("pathIsEnded")] [HideInInspector]
    public bool reachedEndOfPath = false;

    // The waypoint we are currently moving towards
    private int currentWaypoint = 0;

    // The max distance from the AI to a waypoint for it to continure to the next waypoint
    public float nextWaypointDistance = 3f;

    private void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        if (target == null)
        {
            Debug.LogError("No Player found? PANIC!");
            return;
        }
        
        // Start a new path to the target position, return the result to the OnPathComplete method
        

        InvokeRepeating("UpdatePath", 0f, .5f);
    }

    private IEnumerator UpdatePath()
    {
        if (target == null)
        {
            // TODO: Insert a player search here
            yield return false;
        }
        if (seeker.IsDone())
            seeker.StartPath(rb.position, target.position, OnPathComplete);
    }

    public void OnPathComplete(Path p)
    {
        Debug.Log("We got a path. Did it have an error?" + p.error);
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    private void FixedUpdate()
    {
        if (target == null)
        {
            // TODO: Insert a player search here
            return ;
        } 
        
        // TODO: Always look at player

        if (path == null)
        {
            return;
        }

        if (currentWaypoint >= path.vectorPath.Count) 
        {
            reachedEndOfPath = true;
            return;
        }

        reachedEndOfPath = false;
        
        // Direction to the next waypoint
        
        Vector2 dir = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = Time.deltaTime * speed * dir;
        
        //dir *= speed * Time.fixedDeltaTime;  // TODO: problem with Game.deltaWorld, static?
        
        // Move the AI
        rb.AddForce(force);

        float dist = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (dist < nextWaypointDistance)
        {
            currentWaypoint++;
            return;
        }
    }
}
