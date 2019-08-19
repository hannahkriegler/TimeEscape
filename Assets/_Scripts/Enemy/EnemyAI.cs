using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
using Pathfinding;
using TE;
using UnityEngine.Serialization;

namespace TE
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Seeker))]
    public class EnemyAI : MonoBehaviour
    {
        public Transform target;

        // How many times each second we will update our path
        public float updateRate = 2f;

        // Flip to right direction
        public Transform enemyGFX;
        public float rotationSpeed;

        private Seeker seeker;
        private Rigidbody2D rb;
        public Path path;

        // the AI's speed per second
        public float speed = 200f;

        [FormerlySerializedAs("pathIsEnded")]
        [HideInInspector]
        public bool reachedEndOfPath = false;

        private int currentWaypoint = 0;
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
            InvokeRepeating("UpdatePath", 0f, .5f);
        }

        private void UpdatePath()
        {
            if (target == null)
            {
                // TODO: Insert a player search here
                return;
            }
            if (seeker.IsDone())
                seeker.StartPath(rb.position, target.position, OnPathComplete);
        }

        public void OnPathComplete(Path p)
        {
            //Debug.Log("We got a path. Did it have an error?" + p.error);
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
                return;
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

            if (force.x <= 0.01)
            {
                enemyGFX.localScale = new Vector3(-Mathf.Abs(enemyGFX.localScale.x), enemyGFX.localScale.y, enemyGFX.localScale.z);
            }
            else if (force.x >= -0.01f)
            {
                enemyGFX.localScale = new Vector3(Mathf.Abs(enemyGFX.localScale.x), enemyGFX.localScale.y, enemyGFX.localScale.z);
            }


        }
    }
}