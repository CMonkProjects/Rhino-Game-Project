using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerWave : EnemySpawner
{
    [Header("Percentage of enemies remaining in wave before starting next wave")]
    [SerializeField] float waveConditions = 0f;
    float startingEnemyCount;
    public bool waveNeedsDefeating = false;
    bool waveDefeated = false;

    // Start is called before the first frame update
    protected override void Start()
    {
        foreach (Transform enemy in transform)
        {
            enemyToSpawn.Add(enemy);
        }

        startingEnemyCount = enemyToSpawn.Count;

        GameEvents.current.onEnemyDestroyed += CheckEnemiesInWave;
    }

    public void OnDisable()
    {
        GameEvents.current.onEnemyDestroyed -= CheckEnemiesInWave;
    }

    public void CheckEnemiesInWave()
    {
        for (var i = enemyToSpawn.Count - 1; i > -1; i--)
        {
            if (enemyToSpawn[i] == null)
                enemyToSpawn.RemoveAt(i);
        }

        GameEvents.current.NewMessage("Enemy Wave count is - " + enemyToSpawn.Count);

        if (enemyToSpawn.Count <= waveConditions)
        {
            GameEvents.current.NewMessage("Wave conditions complete for wave " + gameObject.name);
            waveDefeated = true;
        }
    }

    public bool EnemyWaveDefeated()
    {
        return waveDefeated;
    }
}
