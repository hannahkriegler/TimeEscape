using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    public Color color;
    // Start is called before the first frame update
    void Start()
    {
        ChangeColor();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ChangeColor()
    {
        foreach (SpriteRenderer s in GetComponentsInChildren<SpriteRenderer>())
        {
            s.color = color;
        }
    }
}
