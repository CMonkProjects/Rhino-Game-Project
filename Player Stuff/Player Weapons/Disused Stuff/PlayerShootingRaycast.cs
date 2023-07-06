using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootingRaycast : MonoBehaviour
{
    public string soundEffect = "Shoot";
    public Transform[] gunpoint = new Transform[2];

    public float damage = 1;
    float tracerLength = 5f;
    [SerializeField] LayerMask layerMask;  //layerMask is what objects we want our raycast to hit (in this case enemies and obstacles)

    public LineRenderer lineRenderer;

    //Particle effects
    [SerializeField] protected ParticleSystem[] particleEffect_Gunpoint = new ParticleSystem[2];

    //firing rate
    public float fireRateDefault = 1f;
    public float fireRateOvercharged = 0.5f;
    public float fireRate = 1f;   //shoots once every half second
    protected float readyToFire = 0f;
    public int ammoCost;
    public int ammoCostDefault;
    public int ammoCostOvercharged;

    //gunpoint cycling
    protected int cycle = 0;
    protected int cycleMax = 1;

    public WeaponAnimator[] weaponBarrelAnimator;
    public string hitEffectTag;

    void Awake()
    {
        LineRenderer line = Instantiate(lineRenderer);
        lineRenderer = line;

        lineRenderer.startWidth = 0.02f;
        lineRenderer.endWidth = 0.02f;

        lineRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!PauseMenu.gameIsPaused && GameController.gameIsActive)
        {
            if (Input.GetButton("Fire1"))
            {
                if (readyToFire > 0)
                {
                    readyToFire -= Time.deltaTime;
                    return;
                }

                /*if (turretAmmo)
                {
                    if (turretAmmo.ammo < 1)
                    {
                        return;
                    }
                }*/

                StartCoroutine(Shoot());
            }
        }
    }

    IEnumerator Shoot()
    {
        //reset the fire rate
        readyToFire = fireRate;

        //Shoot out the raycast from the gunpoint
        RaycastHit2D hit = Physics2D.Raycast(gunpoint[cycle].position, gunpoint[cycle].up, tracerLength, layerMask);

        //Play the sound effect
        GameEvents.current.PlaySound(soundEffect);

        //Activate Particle Effects
        if (particleEffect_Gunpoint[cycle] != null)
        {
            particleEffect_Gunpoint[cycle].Play();
        }
        //Play Weapon Barrel Animation
        if (weaponBarrelAnimator[cycle] != null)
        {
            weaponBarrelAnimator[cycle].Animation_Shooting();
        }

        //If our raycast hits something
        if (hit)
        {
            EnemyHealth enemyHealth = hit.transform.GetComponent<EnemyHealth>();

            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }

            //Grab the required impact effect pooled object and set it at the raycast point we hit
            GameObject impactEffect = ObjectPooler.sharedInstance.GetPooledObject(hitEffectTag);
            if (impactEffect != null)
            {
                impactEffect.transform.position = hit.point;
                impactEffect.transform.rotation = Quaternion.identity;
                impactEffect.SetActive(true);
            }

            //draw a line representing the bullet
            lineRenderer.SetPosition(0, gunpoint[cycle].position);
            lineRenderer.SetPosition(1, hit.point);
        }
        else
        {
            //draw a line representing the bullet, if we don't hit anything
            //we just draw a long line out in the direction we're shooting
            lineRenderer.SetPosition(0, gunpoint[cycle].position);
            lineRenderer.SetPosition(1, gunpoint[cycle].position + gunpoint[cycle].up * tracerLength);
        }

        //draw the line renderer representing the bullet tracer
        lineRenderer.enabled = true;
        lineRenderer.sortingOrder = 0;

        //Wait a bit then disable the line renderer again
        yield return new WaitForSeconds(0.02f);

        DisableLaser();

        //cycle to next gunpoint
        cycle++;

        if (cycle > cycleMax)
            cycle = 0;
    }

    void DisableLaser()
    {
        lineRenderer.enabled = false;
    }

    public virtual void Overcharge(bool _isOvercharged)
    {
        switch (_isOvercharged)
        {
            case true:
                fireRate = fireRateOvercharged;
                ammoCost = ammoCostOvercharged;
                //Speed up the weapon barrel animations
                for(int i = 0; i < weaponBarrelAnimator.Length; i++)
                {
                    weaponBarrelAnimator[i].Animation_Overcharged(true);
                }
                break;

            case false:
                fireRate = fireRateDefault;
                ammoCost = ammoCostDefault;
                for (int i = 0; i < weaponBarrelAnimator.Length; i++)
                {
                    weaponBarrelAnimator[i].Animation_Overcharged(false);
                }
                break;
        }
    }

    void OnDestroy()
    {
        if (lineRenderer != null)
            Destroy(lineRenderer);
    }
}
