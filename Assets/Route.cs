using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Route : MonoBehaviour
{
    [SerializeField] private Transform[] controlPoints;

    public Transform start;
    public Transform target;
    public Transform middle;

    private Vector2 gizmosPosition;

    private void Start()
    {
        
    }

    private void OnDrawGizmos()
    {
        for (float t = 0; t <= 1; t += 0.05f)
        {
            gizmosPosition = Mathf.Pow(1 - t, 3) * controlPoints[0].position +
                             3 * Mathf.Pow(1 - t, 2) * t * controlPoints[1].position +
                             3 * (1 - t) * Mathf.Pow(t, 2) * controlPoints[2].position +
                             Mathf.Pow(t, 3) * controlPoints[3].position;
            
            Gizmos.DrawSphere(gizmosPosition, 0.25f);
        }
        
        Gizmos.DrawLine(new Vector2(controlPoints[0].position.x, controlPoints[0].position.y), 
            new Vector2(controlPoints[1].position.x, controlPoints[1].position.y));
        
        Gizmos.DrawLine(new Vector2(controlPoints[2].position.x, controlPoints[2].position.y), 
            new Vector2(controlPoints[3].position.x, controlPoints[3].position.y));
    }

    private void Update()
    {
        controlPoints[0].transform.position = start.transform.position;
        controlPoints[3].transform.position = new Vector2(target.position.x, -4.5f);
        //controlPoints[3].transform.position = target.transform.position;
        //Debug.Log("startposition: " +  start.position);
        //Debug.Log("targetposition: " +  target.position);


        //var x = ((controlPoints[0].transform.position - controlPoints[3].transform.position) ).x * (1.0f / 3.0f);
        //var t = (controlPoints[0].transform.position - controlPoints[3].transform.position);
        //var c = start.position.x + (start.position.x - target.position.x) /2f;
        //Debug.Log("t: " +  t);
        
        //Debug.DrawLine(t, target.position, Color.blue);
        //Debug.DrawLine(t, start.position, Color.cyan);
        //Debug.DrawLine(controlPoints[0].transform.position, controlPoints[3].transform.position, Color.red);
        //Debug.Log(x);
        controlPoints[1].transform.position = middle.position;
        controlPoints[2].transform.position = middle.position; //new Vector2(c, controlPoints[2].transform.position.y);
    }
}
