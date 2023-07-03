using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManagerSimple : MonoBehaviour
{
    public List<EnemySpawnWaveParent> enemySpawnWaveParent;

    IEnumerator SpawnGroup(int _specifiedSpawner)
    {
        yield return new WaitForSeconds(0.5f);

        enemySpawnWaveParent[_specifiedSpawner].SpawnWave();
    }

    public void SpawnEnemyGroup(int _specifiedSpawner)
    {
        StartCoroutine(SpawnGroup(_specifiedSpawner));
    }
}
