using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    public float scrollingLength_X;
    public float scrollingLength_Y;

    public float scrollSpeed_X;
    public float scrollSpeed_Y;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector2(Mathf.Repeat(Time.time * scrollSpeed_X, scrollingLength_X), Mathf.Repeat(Time.time * scrollSpeed_Y, scrollingLength_Y));
    }
}