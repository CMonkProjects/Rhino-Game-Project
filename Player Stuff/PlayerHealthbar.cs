using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthbar : MonoBehaviour
{
    public Slider slider;
    public Color low;
    public Color high;
    public Image fillImage;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void UpdateHealth(float health, float maxHealth)
    {
        slider.value = health / maxHealth;

        fillImage.color = Color.Lerp(low, high, slider.normalizedValue);  //Set color of healthbar based on remaining health
    }
}
