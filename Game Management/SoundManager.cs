using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;
using UnityEngine.SceneManagement;

//This script will handle most of our sounds (derived from Brackey's sound tutorial)
public class SoundManager : MonoBehaviour
{
    public Sound[] sounds;
    public float masterVolume;

    string currentRepeatSound;
    int currentRepeatTimes;

    public static SoundManager instanceSoundManager;

    Camera mainCamera;

    void Awake()
    {
        /*//Singleton (commented out as it's simpler to just have seperate audios per scene then have one that manages all of them)
        if (instanceSoundManager == null)
        {
            instanceSoundManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }*/

        //Creates an audiosource for every sound we have in the list and sets its name, volume, and pitch
        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();  //creates a new audiosource component but hides it in the inspector (check Sound script)
            s.source.clip = s.clip;
            s.source.volume = s.volume * masterVolume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.ignoreListenerPause = s.ignorePause;   //These sounds will still play even if game is paused (used for menu sounds and music)
        }
    }

    void Start()
    {
        //LoadOptionSettings();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        GameEvents.current.onPlaySound += PlaySound;
        GameEvents.current.onStopSound += StopSound;
        GameEvents.current.onSetVolume += SetVolume;
        GameEvents.current.onPlaySoundMultipleTimes += PlaySoundMultipleTimes;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        GameEvents.current.onPlaySound -= PlaySound;
        GameEvents.current.onStopSound -= StopSound;
        GameEvents.current.onSetVolume -= SetVolume;
        GameEvents.current.onPlaySoundMultipleTimes -= PlaySoundMultipleTimes;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded: " + scene.name);
        Debug.Log(mode);

        mainCamera = Camera.main;
    }

    public void PlaySound(string _name)
    {
        //Find the sound we want and play it
        Sound s = Array.Find(sounds, sound => sound.name == _name);

        if (s == null)
        {
            Debug.LogWarning("Sound: " + _name + " not found!");
            return;
        }

        //Adjust the pitch of the sound effect if set to true
        if (s.addRandomPitch)
        {
            s.source.pitch = s.pitch + UnityEngine.Random.Range(-s.randomPitchAdjustor, s.randomPitchAdjustor);
        }

        s.source.Play();
    }

    void PlaySoundMultipleTimes(string _name, int _numberOfTimes)
    {
        currentRepeatSound = _name;
        currentRepeatTimes = _numberOfTimes;

        StartCoroutine(PlaySoundXTimes());
    }

    IEnumerator PlaySoundXTimes()
    {
        //Find the sound we want and play it
        Sound s = Array.Find(sounds, sound => sound.name == currentRepeatSound);
        int repeatTimes = currentRepeatTimes;

        currentRepeatSound = "";
        currentRepeatTimes = 0;

        if (s == null)
        {
            Debug.LogWarning("Can't find Repeat Sound");
            yield return null;
        }

        for (int i = 0; i < repeatTimes; i++)
        {
            yield return new WaitWhile(() => s.source.isPlaying);

            s.source.Play();
        }
    }

    public void StopSound(string _name)
    {
        //Find the sound we want and stop it
        Sound s = Array.Find(sounds, sound => sound.name == _name);

        if (s == null)
        {
            Debug.LogWarning("Sound: " + _name + " not found!");
            return;
        }

        s.source.Stop();
    }

    //Called by a volume slider in menu
    public void SetVolume(float _newVolume)
    {
        masterVolume = _newVolume;

        foreach(Sound s in sounds)
        {
            s.source.volume = s.volume * masterVolume;
            Debug.Log("New Volume is = " + masterVolume);
        }
    }

    //Save the player's option settings (in this case just the volume)
    /*public void SaveOptionSettings()
    {
        if (PlayerPrefs.HasKey("Volume"))
        {
            PlayerPrefs.SetFloat("Volume", masterVolume);
            Debug.Log("Volume settings saved: " + masterVolume);
        }

        PlayerPrefs.Save();
    }*/

    /*public void LoadOptionSettings()
    {
        if (PlayerPrefs.HasKey("Volume"))
        {
            masterVolume = PlayerPrefs.GetFloat("Volume");
            Debug.Log("Saved volume options loaded");
            SetVolume(masterVolume);
        }
        else
        {
            masterVolume = PlayerPrefs.GetFloat("Volume", 1f);
            PlayerPrefs.SetFloat("Volume", masterVolume);
            SetVolume(masterVolume);
            Debug.Log("Default volume options loaded");
        }
    }*/

    void LateUpdate()
    {
        if (mainCamera != null)
        {
            transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, mainCamera.transform.position.z);
        }
    }
}
