using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponRaycast : PlayerWeaponGeneric
{
    //weapon stuff
    public float damage = 1;
    float tracerLength = 4f;
    [SerializeField] LayerMask layerMask;  //layerMask is what objects we want our raycast to hit (in this case enemies and obstacles)

    public LineRenderer lineRenderer;

    public float fireRate = 1f;   //shoots once every half second
    protected float fireRateTimer = 0f;

    //gunpoint cycling
    protected int cycle = 0;
    protected int cycleMax = 1;

    public string hitEffectTag;

    protected override void Awake()
    {
        base.Awake();

        LineRenderer line = Instantiate(lineRenderer);
        lineRenderer = line;

        lineRenderer.startWidth = 0.02f;
        lineRenderer.endWidth = 0.02f;

        lineRenderer.enabled = false;
    }

    void OnEnable()
    {
        fireRateTimer = fireRate;

        playerWeaponAmmo.UpdateAmmo(ammoIndex, 0);

        GameEvents.current.PlaySound(soundWeaponActivation);
    }

    // Update is called once per frame
    void Update()
    {
        if (!PauseMenu.gameIsPaused && GameController.gameIsActive)
        {
            if (fireRateTimer > 0)
            {
                fireRateTimer -= Time.deltaTime;
            }

            if (Input.GetButton("Fire1"))
            {
                if (fireRateTimer > 0)
                {
                    return;
                }

                if (playerWeaponAmmo.ammoList[ammoIndex].ammoCurrent <= 0)
                {
                    return;
                }

                StartCoroutine(Shoot());
            }
        }
    }

    IEnumerator Shoot()
    {
        //reset the fire rate
        fireRateTimer = fireRate;

        //Shoot out the raycast from the gunpoint
        RaycastHit2D hit = Physics2D.Raycast(gunpoint[cycle].position, gunpoint[cycle].up, tracerLength, layerMask);

        //Play the sound effect
        GameEvents.current.PlaySound(soundWeaponFiring);

        //Activate Particle Effects
        if (particleEffect_Gunpoint[cycle] != null)
        {
            particleEffect_Gunpoint[cycle].Play();
        }

        if (playerWeaponSelector)
        {
            playerWeaponSelector.WeaponSwitchDelay();
        }

        //If our raycast hits something
        if (hit)
        {
            EnemyHealth enemyHealth = hit.transform.GetComponent<EnemyHealth>();

            DestructableHealth treeHealth = hit.transform.GetComponent<DestructableHealth>();

            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }

            if (treeHealth != null)
            {
                treeHealth.TakeDamage(damage);
            }

            //Grab the required impact effect pooled object and set it at the raycast point we hit
            GameObject impactEffect = ObjectPooler.sharedInstance.GetPooledObject(hitEffectTag);

            if (impactEffect != null)
            {
                impactEffect.transform.position = hit.point;
                impactEffect.transform.rotation = gunpoint[cycle].transform.rotation;
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

        playerWeaponAmmo.UpdateAmmo(ammoIndex, 0);  //unlimited ammo
    }

    void DisableLaser()
    {
        lineRenderer.enabled = false;
    }

    void OnDestroy()
    {
        if (lineRenderer != null)
            Destroy(lineRenderer);
    }
}