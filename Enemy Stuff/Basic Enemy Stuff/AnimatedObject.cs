using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This just plays an animation, it is called by the enemy that references it
public class AnimatedObject : MonoBehaviour
{
    public string animationString;

    Animator spriteAnimator;

    void Awake()
    {
        spriteAnimator = GetComponent<Animator>();
    }
    internal void PlayAnimation()
    {
        if (spriteAnimator != null)
        {
            spriteAnimator.Play(animationString, -1, -1);
        }
    }
}
