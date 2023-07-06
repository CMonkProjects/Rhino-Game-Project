using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//https://www.youtube.com/watch?v=5NViMw-ALAo
public class UI_HealthUnit : MonoBehaviour
{
    public Sprite fullHealth, emptyHealth;
    Image healthUnitImage;

    void Awake()
    {
        healthUnitImage = GetComponent<Image>();
    }

    public void SetHealthUnitImage(HealthStatus status)
    {
        switch (status)
        {
            case HealthStatus.Empty:
                healthUnitImage.sprite = emptyHealth;
                break;

            case HealthStatus.Full:
                healthUnitImage.sprite = fullHealth;
                break;
        }
    }
}

public enum HealthStatus
{
    Empty = 0,
    Full = 1
}
