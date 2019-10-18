using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Route draws the path the shroomie spit will take, it is just for debugging
/// </summary>
public class Route : MonoBehaviour
{
    [SerializeField] private Transform[] controlPoints;

    public Transform start;
    public Transform target;
    public Transform middle;

    private float randomHeight;

    private Vector2 gizmosPosition;

/// <summary>
/// Just to see the path for debugging issues
/// </summary>
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

    public void Calculate()
    {
        controlPoints[0].transform.position = start.transform.position;
        controlPoints[3].transform.position = new Vector2(target.position.x, -6f);

        var middlePos = start.transform.position + (target.transform.position - start.transform.position) * 0.3f;
        
        randomHeight = Random.Range(middle.position.y - 0.2f, middle.position.y + 0.8f);
        var position = new Vector2(middlePos.x, randomHeight);
        controlPoints[1].transform.position = position;
        controlPoints[2].transform.position = position; 
    }
}
