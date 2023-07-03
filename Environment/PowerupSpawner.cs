using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupSpawner : MonoBehaviour
{
    [SerializeField] GameObject powerupToSpawn;
    GameObject referencedPowerup;

    float timer;
    float timeToCheck = 1f;
    [SerializeField] float spawningTime = 3f;
    bool spawningNewPowerup = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnNewPowerup());
        timer = timeToCheck;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            CheckForPowerup();
        }
    }

    void CheckForPowerup()
    {
        if (referencedPowerup == null && !spawningNewPowerup)
        {
            StartCoroutine(SpawnNewPowerup());
        }

        timer = timeToCheck;
    }

    IEnumerator SpawnNewPowerup()
    {
        spawningNewPowerup = true;

        yield return new WaitForSeconds(spawningTime);

        referencedPowerup = Instantiate(powerupToSpawn, transform.position, transform.rotation);

        spawningNewPowerup = false;
    }
}
