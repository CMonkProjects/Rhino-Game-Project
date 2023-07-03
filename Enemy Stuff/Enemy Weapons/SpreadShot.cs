using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpreadShot : MonoBehaviour
{
    public EnemyWeapon enemyWeapon;

    public int numberOfShots;

    public float[] spreadAngle;

    // Start is called before the first frame update
    void Start()
    {
        ShootPellets();
    }

    void ShootPellets()
    {
        for (int i = 0; i < numberOfShots; i++)
        {
            float spreadRotation = spreadAngle[i];

            GameObject bullet = Instantiate(enemyWeapon.projectilePrefab,
            transform.position,
            transform.rotation);

            //Add the spread angle rotation to the projecitle
            bullet.transform.Rotate(0f, 0f, spreadRotation);

            Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();

            bulletRB.AddForce(bullet.transform.up * enemyWeapon.projectileForce, ForceMode2D.Impulse);
        }

        Destroy(gameObject);
    }
}
