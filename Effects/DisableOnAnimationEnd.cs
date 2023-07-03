using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableOnAnimationEnd : MonoBehaviour
{
    void DisableAnimationEnd()
    {
        gameObject.SetActive(false);
    }
}
