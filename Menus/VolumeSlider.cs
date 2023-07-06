using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] Slider slider;
    public SoundManager soundManager;
    void Awake()
    {
        slider = GetComponent<Slider>();
    }

    private void OnEnable()
    {
        if (soundManager)
        {
            slider.value = soundManager.masterVolume;
        }
    }

    public void SetVolume()
    {
        float _volume = 0f;

        _volume = slider.value;
        GameEvents.current.SetVolume(_volume);
    }
}
