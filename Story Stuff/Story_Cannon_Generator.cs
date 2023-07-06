using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Story_Cannon_Generator : MonoBehaviour
{
    [SerializeField] DestructableHealth destructableHealth;

    [SerializeField] string messageWhenDestroyed;

    void OnEnable()
    {
        if (destructableHealth != null)
            destructableHealth.onDestroyed += Destroyed;
    }

    void OnDisable()
    {
        if (destructableHealth != null)
            destructableHealth.onDestroyed -= Destroyed;
    }

    void Destroyed()
    {
        GameEvents.current.GeneratorDestroyed();

        GameEvents.current.NewMessage(messageWhenDestroyed);
    }
}
