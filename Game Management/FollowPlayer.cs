using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] Transform player;

    void OnEnable()
    {
        GameEvents.current.onPlayerSpawned += StartFollowingPlayer;
    }

    void OnDisable()
    {
        GameEvents.current.onPlayerSpawned -= StartFollowingPlayer;
    }

    void LateUpdate()
    {
        if (player != null)
        {
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
        }
        else
        {
            StartFollowingPlayer();
        }
    }

    void StartFollowingPlayer()
    {
        if (player == null)
        {
            player = GameObject.Find("Player Rhino(Clone)").transform;
        }
    }
}
