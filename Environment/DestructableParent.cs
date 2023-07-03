using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableParent : MonoBehaviour
{
    [SerializeField] DestructableHealth destructableHealth;

    void OnEnable()
    {
        if (destructableHealth == null)
            destructableHealth = transform.GetChild(0).GetComponent<DestructableHealth>();

        if (destructableHealth != null)
            destructableHealth.onDestroyed += Destroyed;
    }

    void OnDisable()
    {
        if (destructableHealth != null)
            destructableHealth.onDestroyed -= Destroyed;
    }


    //Destroy function
    void Destroyed()
    {
        Destroy(gameObject, 0.1f);
    }
}
