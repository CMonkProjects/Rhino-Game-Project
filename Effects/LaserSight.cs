using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSight : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public float lineStartWidth = 0.01f;
    public float lineEndWidth = 0.01f;

    [SerializeField] protected float tracerLength = 1f;
    [SerializeField] protected LayerMask layerMask;  //layerMask is what objects we want our raycast to hit (enemies and obstacles)
    [SerializeField] protected int sortingOrder = 5;

    protected virtual void Awake()
    {
        LineRenderer line = Instantiate(lineRenderer);
        lineRenderer = line;

        lineRenderer.startWidth = lineStartWidth;
        lineRenderer.endWidth = lineEndWidth;

        lineRenderer.enabled = false;
    }

    protected virtual void OnEnable()
    {
        StartCoroutine(Laser());
    }

    protected virtual void OnDisable()
    {
        if (lineRenderer != null)
            lineRenderer.enabled = false;
    }

    protected virtual IEnumerator Laser()
    {
        while (true)
        {
            //Shoot out the raycast
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, tracerLength, layerMask);

            //If our raycast hits something
            if (hit)
            {
                //draw the line representing the laser
                lineRenderer.SetPosition(0, transform.position);
                lineRenderer.SetPosition(1, hit.point);

            }
            else
            {
                //draw a line representing the laser if we don't hit anything
                lineRenderer.SetPosition(0, transform.position);
                lineRenderer.SetPosition(1, transform.position + transform.up * tracerLength);
            }

            //draw the line renderer representing the laser
            lineRenderer.enabled = true;
            lineRenderer.sortingOrder = sortingOrder;
            //Wait till next frame
            yield return null;
        }
    }
}
