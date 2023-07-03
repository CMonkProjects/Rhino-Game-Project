using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmoDraw : MonoBehaviour
{
    public Color color = Color.green;

    void OnDrawGizmos()
    {
        Gizmos.color = color;

        color.a = 0.5f;

        Gizmos.DrawCube(GetComponent<BoxCollider2D>().bounds.center, GetComponent<BoxCollider2D>().bounds.size);
    }
}
