using UnityEngine;
using System.Collections;

//This is the base turret behavior for all turrets

public class EnemyTurret : MonoBehaviour
{
    public Transform currentTarget;
    [SerializeField] protected Transform aiParent;

    [SerializeField] protected bool targetInViewCone;
    [SerializeField] protected internal bool hasLineOfSight;

    [SerializeField] protected float rotationSpeed = 90f;

    [SerializeField] protected LayerMask obstacleMask;  //layerMask to check if obstacles like walls are in the way of the player target

    //Array of weapons the turret has
    public EnemyWeapon[] enemyWeapon;

    [SerializeField] protected float attackTimeMax = 2f;
    protected float attackTimer = 0f;
    [SerializeField] protected internal bool canAttack = true;
    [SerializeField] protected bool turretActive = true;

    protected internal int currentWeaponIndex = 0;

    //Materials and animation
    [SerializeField] protected Material matYellow;
    [SerializeField] protected Material matBlue;
    [SerializeField] protected Material matDefault;
    [SerializeField] protected SpriteRenderer spriteRenderer;

    protected Coroutine shootingCoroutine;

    protected EnemyHealth enemyHealth;
    protected ObjectCleanup objectCleanup;
    
    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        matYellow = Resources.Load("YellowFlash", typeof(Material)) as Material;
        matBlue = Resources.Load("BlueFlash", typeof(Material)) as Material;
        matDefault = spriteRenderer.material;
        aiParent = transform.parent;
        ResetWeaponRecoil();
    }

    protected virtual void OnEnable()
    {
        enemyHealth = transform.parent.GetChild(0).GetComponent<EnemyHealth>();
        objectCleanup = transform.parent.GetComponent<ObjectCleanup>();

        if (enemyHealth != null)
        {
            enemyHealth.onDying += StopShooting;        //listen to the delegates
            enemyHealth.onDead += StopShooting;

            enemyHealth.onDestroyed += Destroy;

            enemyHealth.onDamageFlash += DamageFlash;

            enemyHealth.onResetMaterial += ResetMaterial;
        }

        if (objectCleanup != null)
        {
            objectCleanup.onDestroyed += Destroy;
        }
    }

    protected virtual void OnDisable()
    {
        if (enemyHealth != null)
        {
            enemyHealth.onDying -= StopShooting;
            enemyHealth.onDead -= StopShooting;

            enemyHealth.onDestroyed -= Destroy;

            enemyHealth.onDamageFlash += DamageFlash;

            enemyHealth.onResetMaterial += ResetMaterial;
        }

        if (objectCleanup != null)
        {
            objectCleanup.onDestroyed -= Destroy;
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (!PauseMenu.gameIsPaused && GameController.gameIsActive)
        {
            //If turret is inactive then don't move or shoot (only happens when enemy is being destroyed)
            if (!turretActive)
            {
                return;
            }

            if (currentTarget != null)
            {
                //Direction to rotate towards target
                float angle = Mathf.Atan2(currentTarget.position.y - transform.position.y, currentTarget.position.x - transform.position.x) * Mathf.Rad2Deg - 90f;

                Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));

                //Rotate the turret towards the target
                transform.localRotation = Quaternion.RotateTowards(transform.localRotation, targetRotation, rotationSpeed * Time.deltaTime);

                if (!canAttack)
                    return;

                if (attackTimer > 0)
                {
                    attackTimer -= Time.deltaTime;
                    return;
                }

                if (targetInViewCone && hasLineOfSight && canAttack)
                {
                    StartCoroutine(StartShooting());
                }
            }
            else
            {
                //Rotate back towards default rotation
                if (transform.parent == null)
                {
                    return;
                }

                Quaternion defaultRotation = transform.parent.localRotation;

                transform.rotation = Quaternion.RotateTowards(transform.rotation, defaultRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }

    //If target is within sightbox and raycast has no wall between turret and player, then turret can start shooting
    protected virtual void FixedUpdate()
    {
        if (currentTarget == null)
        {
            return;
        }

        //Shoot a linecast towards the player to check if target isn't behind a wall, otherwise he's got line of sight
        if (!Physics2D.Linecast(transform.position, currentTarget.position, obstacleMask))
        {
            hasLineOfSight = true;
            Debug.DrawLine(transform.position, currentTarget.position, Color.yellow);
        }
        else hasLineOfSight = false;
    }

    //Due to turret rotation issues while being a child object, we detach the turret object so it can rotate independantly (when player is spotted)
    protected virtual void LateUpdate()
    {
        if (currentTarget != null)
        {
            if (transform.parent != null)
            {
                //Now the turret can rotate freely without its accuracy being thrown off every time the parent turns left or right
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
            //When enemy loses track of player the turret reattaches to the parent
            if (transform.parent == null)
            {
                if (aiParent != null)
                {
                    transform.parent = aiParent;
                }
                else Destroy(gameObject);
            }
        }
    }

    protected virtual void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            targetInViewCone = true;
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            targetInViewCone = false;
        }
    }

    public virtual void SetCurrentTarget(Transform _currentTarget)
    {
        currentTarget = _currentTarget;
    }

    protected internal virtual IEnumerator StartShooting()
    {
        canAttack = false;

        //Wait for this coroutine to finish before moving on
        yield return shootingCoroutine = StartCoroutine(Shooting(currentWeaponIndex));

        yield return new WaitForSeconds(0);

        ResetWeapon();
    }

    //Shoot the currently selected weapon
    //Weapons can fire in salvos, or groups of shots, so we can have more flexible shooting patterns for different weapons
    protected internal virtual IEnumerator Shooting(int _currentWeaponIndex)
    {
        EnemyWeapon currentWeapon = enemyWeapon[_currentWeaponIndex];

        if (currentWeapon != enemyWeapon[currentWeaponIndex])
            yield break;

        int _ammo = currentWeapon.shotsPerSalvo;
        int _salvo = currentWeapon.numberOfSalvos;

        int _cycle = 0;
        int _cycleMax = currentWeapon.gunpoint.Length - 1;

        //If our enemy is using a homing weapon then it assigns the target to the projectile
        if (currentWeapon.weaponIsHoming)
        {
            HomingProjectileBehavior homingMissileBehavior = currentWeapon.projectilePrefab.GetComponent<HomingProjectileBehavior>();

            homingMissileBehavior.SetTarget(currentTarget);
        }

        //Random wait time
        float waitTime = Random.Range(0.05f, 0.5f);

        yield return new WaitForSeconds(currentWeapon.waitToFire + waitTime);

        //----------------------------------------------------------------------------------------------
        //Fire all the salvos when attacking
        for (int i = _salvo; i > 0; i--)
        {
            //Individual salvo firing
            for (int j = _ammo; j > 0; j--)
            {
                if (!turretActive)
                {
                    break;
                }

                GameObject projectile = Instantiate(currentWeapon.projectilePrefab, 
                currentWeapon.gunpoint[_cycle].position, 
                currentWeapon.gunpoint[_cycle].rotation);

                //Add various adjustments to the fired projectile
                currentWeapon.AdjustProjectile(projectile, _cycle);

                _cycle++;

                if (_cycle > _cycleMax)
                {
                    _cycle = 0;
                }

                yield return new WaitForSeconds(currentWeapon.fireRate);
            }
            
            //When salvo is done then start firing the next one
            yield return new WaitForSeconds(currentWeapon.waitBetweenSalvo);
        }
        //----------------------------------------------------------------------------------------------
    }

    protected internal virtual void ResetWeapon()
    {
        attackTimer = attackTimeMax;
        canAttack = true;

        ResetWeaponRecoil();
    }

    protected internal virtual void ResetWeaponRecoil()
    {
        EnemyWeapon currentWeapon = enemyWeapon[currentWeaponIndex];

        currentWeapon.ResetRecoil();
    }

    protected internal virtual void DamageFlash(Material material)
    {
        spriteRenderer.material = material;
    }

    protected internal virtual void ResetMaterial()
    {
        spriteRenderer.material = matDefault;
    }

    //Turret stops shooting when enemy is dead or dying
    protected internal virtual void StopShooting()
    {
        TurretActive(false);
    }

    protected internal virtual void TurretActive(bool _turretActive)
    {
        turretActive = _turretActive;

        //StopCoroutine(shootingCoroutine);
    }

    protected virtual internal void Destroy()
    {
        Destroy(gameObject);
    }
}
