using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketSwerve : MonoBehaviour
{
    public float sinSpeed;
    public float sinMagnitude;
    public float randomRange;

    void Start()
    {
        sinSpeed += Random.Range(-randomRange, randomRange);
        sinMagnitude += Random.Range(-randomRange, randomRange);
    }

    void Update()
    {
        transform.localPosition = new Vector2(Sine(), 0.15f);
    }

    float Sine()
    {
        return sinMagnitude * Mathf.Sin(Time.time * sinSpeed);
    }
}
