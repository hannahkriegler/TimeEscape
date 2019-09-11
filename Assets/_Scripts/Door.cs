using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Vector2 down;
    private Vector2 up;

    [HideInInspector] 
    public bool isDown = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveDoor(bool down)
    {
        if (down)
        {
            Debug.Log("Moved Door Down");
            isDown = true;
        }
        else
        {
            Debug.Log("Moved Door Up");

            isDown = false;
        }
    }

    private void MoveTo(Vector2 target)
    {
        
    }
    
    
}
