using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public Transform[] gunpoint = new Transform[1];
    public GameObject bulletPrefab;

    public float bulletForce = 20f;

    int cycle = 0;
    int cycleMax = 1;

    // Update is called once per frame
    void Update()
    {
        if (!PauseMenu.gameIsPaused && GameController.gameIsActive)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, gunpoint[cycle].position, gunpoint[cycle].rotation);
        Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();

        bulletRB.AddForce(gunpoint[cycle].up * bulletForce, ForceMode2D.Impulse);

        cycle++;

        if (cycle > cycleMax)
            cycle = 0;
    }
}
