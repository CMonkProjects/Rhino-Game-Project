using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class EnemySpawnWaveManager : MonoBehaviour
{
    public List<EnemySpawnWaveParent> enemySpawnWaves;

    [SerializeField] public static int enemyCount = 0;
    [SerializeField] public static bool waveActive = false;

    public static int currentWave = 0;

    float timer = 0f;
    float waveTimer = 0.5f;

    //For keeping enemy mobiles from colliding into each other
    [SerializeField] public static int enemyPriorityCounter = 0;

    public string enemyWaveDestroyed;
    public string startingNewWave;

    public UnityEvent onEnemyWaveDestroyed;
    public UnityEvent onBeginCountdown;
    public UnityEvent onStartNewWave;
    public UnityEvent onCheckForEnemies;
    public UnityEvent onAllWavesDefeated;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (waveActive)
        {
            timer += Time.deltaTime;

            if (timer >= waveTimer)
            {
                CheckForEnemies();
                timer = 0f;
            }
        }
    }

    void CheckForEnemies()
    {
        onCheckForEnemies.Invoke();
        //if enemyCount == 0 then allEnemiesAreDead = true
        bool allEnemiesAreDead = enemyCount == 0;

        if (allEnemiesAreDead)
        {
            StartCoroutine(WaveComplete());
        }
    }

    IEnumerator WaveComplete()
    {
        onEnemyWaveDestroyed.Invoke();  //unity event
        waveActive = false;
        
        //short timer before start new wave fires
        yield return new WaitForSeconds(1.5f);

        if (currentWave < enemySpawnWaves.Count - 1)
        {
            currentWave++;
        }
        else
        {
            //all waves complete
            onAllWavesDefeated.Invoke();
            yield break;
        }
            

        //Tell timer script to begin countdown
        onBeginCountdown.Invoke();

    }

    IEnumerator StartNewWave()
    {
        onStartNewWave.Invoke();  //unity event

        //begin countdown
        yield return new WaitForSeconds(1f);

        enemySpawnWaves[currentWave].SpawnWave();
    }

    //Timer script starts new wave when finished
    public void BeginNewWave()
    {
        StartCoroutine(StartNewWave());
    }
}