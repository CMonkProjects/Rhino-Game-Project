using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] protected bool targetPlayer;
    [SerializeField] protected List<Transform> enemyToSpawn;

    [SerializeField] protected string notes;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        foreach(Transform enemy in transform)
        {
            enemyToSpawn.Add(enemy);
        }
    }

    //Spawn enemy function (called by trigger events)
    public virtual void SpawnEnemies()
    {
        //for loop cycles through all the enemy children and activates them
        for (int i = 0; i < enemyToSpawn.Count; i++)
        {
            if (enemyToSpawn[i] != null)
            {
                enemyToSpawn[i].gameObject.SetActive(true);
                //deparent the enemy after 'spawning'
                enemyToSpawn[i].SetParent(null);

                if (enemyToSpawn[i].GetComponent<EnemyBehavior>() != null)
                {
                    if (targetPlayer)
                    {
                        enemyToSpawn[i].GetComponent<EnemyBehavior>().currentPlayerTarget = GameObject.Find("Player Rhino(Clone)").transform;
                    }
                }
            }
        }

        EnemySpawnWaveManager.waveActive = true;
    }
}
