using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    AstarPath astarPath;

    BoxCollider2D collider2D;

    [SerializeField] DestructableHealth destructableHealth;

    void Awake()
    {
        astarPath = GameObject.Find("A* Pathfinding").GetComponent<AstarPath>();

        collider2D = GetComponent<BoxCollider2D>();

        if (destructableHealth == null)
            GetComponent<DestructableHealth>();
    }

    private void OnEnable()
    {
        //subscribe to delegate
        if (destructableHealth != null)
        {
            destructableHealth.onDestroyed += Destroyed;
        }
    }

    private void OnDisable()
    {
        if (destructableHealth != null)
            destructableHealth.onDestroyed -= Destroyed;
    }

    void Destroyed()
    {
        //Update the A* Pathfinding nodegraph when the building is destroyed
        if (astarPath)
        {
            collider2D.enabled = false;

            //This removes the building's nodegraph instead of having to rescan the whole map, which would otherwise cause noticable lag
            astarPath.UpdateGraphs(collider2D.bounds);
        }
    }
}
