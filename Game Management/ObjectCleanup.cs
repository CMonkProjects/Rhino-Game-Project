using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Object gets destroyed when player leaves designated area
public class ObjectCleanup : MonoBehaviour
{
    [SerializeField] string areaName;

    //Delegate
    public delegate void OnDestroyed();
    public OnDestroyed onDestroyed;

    void Start()
    {
        GameEvents.current.onPlayerLeavingArea += PlayerLeavesArea;
    }

    void PlayerLeavesArea(string _areaName)
    {
        if (_areaName == areaName)
        {
            //call delegate
            if (onDestroyed != null)
            {
                onDestroyed();
            }

            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        GameEvents.current.onPlayerLeavingArea -= PlayerLeavesArea;
    }
}
