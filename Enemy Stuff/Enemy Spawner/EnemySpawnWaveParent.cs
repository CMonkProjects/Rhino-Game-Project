using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnWaveParent : MonoBehaviour
{
    [SerializeField] List<EnemySpawnerWave> enemySpawnerWave;
    [SerializeField] int currentSpawnerIndex = 0;

    public float timeBetweenWaves = 3f;

    void Start()
    {
        foreach (Transform childSpawner in transform)
        {
            enemySpawnerWave.Add(childSpawner.GetComponent<EnemySpawnerWave>());
        }
    }

    public void SpawnWave()
    {
        //check if current wave needs to be defeated first before spawning next one
        if (enemySpawnerWave[currentSpawnerIndex].waveNeedsDefeating)
        {
            if (enemySpawnerWave[currentSpawnerIndex].EnemyWaveDefeated())
            {
                Invoke("SpawnNextWave", timeBetweenWaves);
            }
        }
        else Invoke("SpawnNextWave", timeBetweenWaves);
    }

    void SpawnNextWave()
    {
        enemySpawnerWave[currentSpawnerIndex].SpawnEnemies();

        if (currentSpawnerIndex < enemySpawnerWave.Count - 1)
            currentSpawnerIndex++;
    }

    //Spawn enemy groups
    IEnumerator SpawnWaves()
    {
        //spawn enemies
        for (int i = 0; i < enemySpawnerWave.Count; i++)
        {
            GameEvents.current.NewMessage("Spawning wave - " + i);

            enemySpawnerWave[i].SpawnEnemies();

            //stop coroutine while game is paused
            yield return new WaitWhile(() => PauseMenu.gameIsPaused && !GameController.gameIsActive && Time.timeScale == 0f);

            yield return new WaitForSeconds(timeBetweenWaves);
        }

    }
}