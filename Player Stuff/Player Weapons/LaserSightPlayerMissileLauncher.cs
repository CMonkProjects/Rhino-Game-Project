using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSightPlayerMissileLauncher : LaserSight
{
    [SerializeField] Transform targetPos;
    protected override IEnumerator Laser()
    {
        while (true)
        {
            Vector3 target = targetPos.transform.position;

            //Shoot out the raycast
            RaycastHit2D hit = Physics2D.Raycast(transform.position, target - transform.position);
            //RaycastHit2D hit = Physics2D.Raycast(transform.position, laserTarget.transform, tracerLength, layerMask);

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
