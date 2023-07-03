using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTurret : MonoBehaviour
{
    [SerializeField] protected Transform currentTarget;
    [SerializeField] protected Transform aiParent;

    [SerializeField] protected float rotationSpeed = 90f;

    [SerializeField] protected bool canAttack = true;
    [SerializeField] protected bool turretActive = true;
    [SerializeField] protected bool followTarget;
    [SerializeField] protected bool turretIsShooting;

    //Array of weapons the turret has
    public EnemyWeapon[] enemyWeapon;

    [SerializeField] protected int currentWeaponIndex = 0;

    //Materials
    protected Material matDefault;
    protected SpriteRenderer spriteRenderer;

    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        matDefault = spriteRenderer.material;
        aiParent = transform.parent;
    }

    void LateUpdate()
    {
        if (currentTarget != null)
        {
            if (transform.parent != null)
            {
                gameObject.transform.parent = null;
                transform.position = aiParent.position;
                return;
            }

            if (aiParent != null)
            {
                transform.position = aiParent.position;
            }
        }
        else
        {
            if (transform.parent == null)
            {
                if (aiParent != null)
                {
                    transform.parent = aiParent;
                }
            }
        }
    }

    //Rotate Towards Target Coroutine
    internal IEnumerator RotateTurretTowardsTarget()
    {
        StartCoroutine(StartShooting());

        while (turretIsShooting)
        {
            //Direction to rotate towards target
            float angle = Mathf.Atan2(currentTarget.position.y - transform.position.y, currentTarget.position.x - transform.position.x) * Mathf.Rad2Deg - 90f;
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
            //Rotate the turret towards the target
            transform.localRotation = Quaternion.RotateTowards(transform.localRotation, targetRotation, rotationSpeed * Time.deltaTime);

            yield return null;
        }
    }

    internal IEnumerator TurretSpinAttack()
    {
        float duration = 5f;
        float startRotation = transform.eulerAngles.z;
        float endRotation = startRotation + 360.0f;
        float t = 0.0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float zRotation = Mathf.Lerp(startRotation, endRotation, t / duration) % 360.0f;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y,
            zRotation);
            yield return null;
        }
    }

    //Shooting Coroutine
    internal IEnumerator StartShooting()
    {
        turretIsShooting = true;

        EnemyWeapon currentWeapon = enemyWeapon[currentWeaponIndex];

        int _ammo = currentWeapon.shotsPerSalvo;
        int _salvo = currentWeapon.numberOfSalvos;

        //check if target is in viewcone

        //viewcone code

        //Fire all the salvos when attacking
        for (int i = _salvo; i > 0; i--)
        {
            //Individual salvo firing
            for (int j = _ammo; j > 0; j--)
            {
                ShootWeapon();

                yield return new WaitForSeconds(currentWeapon.fireRate);
            }

            //When salvo is done then start firing the next one
            yield return new WaitForSeconds(currentWeapon.waitBetweenSalvo);
        }

        //Turret stops tracking and current attack state knows to end
        turretIsShooting = false;
    }

    //Shoot current weapon function
    internal void ShootWeapon()
    {
        EnemyWeapon currentWeapon = enemyWeapon[currentWeaponIndex];

        float randomRotation = Random.Range(-currentWeapon.recoilCurrent, currentWeapon.recoilCurrent);

        GameObject bullet = Instantiate(currentWeapon.projectilePrefab,
        currentWeapon.gunpoint[0].position,
        currentWeapon.gunpoint[0].rotation);

        //Add some random rotation to the instantiated projectile
        bullet.transform.Rotate(0f, 0f, randomRotation);

        //Play the particle effect if we have one
        if (currentWeapon.particleEffect_Gunpoint[0] != null)
        {
            currentWeapon.particleEffect_Gunpoint[0].Play();
        }
        //Play shooting animation if we have one
        if (currentWeapon.weaponAnimator[0] != null)
        {
            currentWeapon.weaponAnimator[0].Animation_Shooting();
        }

        Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();

        if (bulletRB)
        {
            bulletRB.AddForce(bullet.transform.up * currentWeapon.projectileForce, ForceMode2D.Impulse);
        }
    }

    internal bool DoneShooting()
    {
        if (!turretIsShooting)
        {
            return true;
        }
        else return false;
    }

    internal virtual void SwitchToNewTurretWeapon(int _newWeaponIndex)
    {
        currentWeaponIndex = _newWeaponIndex;
    }

    internal virtual void SetNewTarget(Transform _newTarget)
    {
        currentTarget = _newTarget;
    }

    internal virtual void RotateTowardsTarget(bool _bool)
    {
        followTarget = _bool;

        StartCoroutine(RotateTurretTowardsTarget());
    }

    internal virtual void DamageFlash(Material material)
    {
        spriteRenderer.material = material;
    }

    internal virtual void ResetMaterial()
    {
        spriteRenderer.material = matDefault;
    }

    internal virtual void TurretActive(bool _turretActive)
    {
        turretActive = _turretActive;
    }
}
