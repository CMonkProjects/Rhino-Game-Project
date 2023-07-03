using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionTest : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    public Sprite sprite_switchOn;
    public Sprite sprite_switchOff;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite_switchOff;
    }

    public void ChangeSprite()
    {
        if (spriteRenderer != null)
        {
            Debug.Log("Action doing its thing");
            spriteRenderer.sprite = sprite_switchOn;
            GameEvents.current.PlaySound("Powerup");
        }
    }
}
