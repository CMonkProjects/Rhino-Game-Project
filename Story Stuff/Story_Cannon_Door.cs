using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Story_Cannon_Door : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameEvents.current.onAllGeneratorsDestroyed += DoorDeactivated;
    }

    void OnDestroy()
    {
        GameEvents.current.onAllGeneratorsDestroyed -= DoorDeactivated;
    }

    void DoorDeactivated()
    {
        gameObject.SetActive(false);
    }
}
