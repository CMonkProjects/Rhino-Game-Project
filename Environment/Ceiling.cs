using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ceiling : MonoBehaviour
{
    public string id;

    // Start is called before the first frame update
    void Start()
    {
        GameEvents.current.onCeilingTriggerEnter += OnCeilingEnter;
        GameEvents.current.onCeilingTriggerExit += OnCeilingExit;
    }

    void OnCeilingEnter(string id)
    {
        if (id == this.id)
        {
            gameObject.SetActive(false);
        }
    }

    void OnCeilingExit(string id)
    {
        if (id == this.id)
        {
            gameObject.SetActive(true);
        }
    }

    void OnDestroy()
    {
        GameEvents.current.onCeilingTriggerEnter -= OnCeilingEnter;
        GameEvents.current.onCeilingTriggerExit -= OnCeilingExit;
    }
}
