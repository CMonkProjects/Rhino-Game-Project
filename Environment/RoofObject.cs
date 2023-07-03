using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoofObject : MonoBehaviour
{
    public string id;

    [SerializeField] List<SpriteRenderer> spriteRendererList;

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
            //spriteRenderer.enabled = false;

            foreach (SpriteRenderer sprite in spriteRendererList)
            {
                sprite.enabled = false;
            }
        }
    }

    void OnCeilingExit(string id)
    {
        if (id == this.id)
        {
            //spriteRenderer.enabled = true;

            foreach (SpriteRenderer sprite in spriteRendererList)
            {
                sprite.enabled = true;
            }
        }
    }

    void OnDestroy()
    {
        GameEvents.current.onCeilingTriggerEnter -= OnCeilingEnter;
        GameEvents.current.onCeilingTriggerExit -= OnCeilingExit;
    }
}
