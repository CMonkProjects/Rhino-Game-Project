using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class PlayerMissileTargeting : MonoBehaviour
{
    [SerializeField] LayerMask layerMask;
    [SerializeField] LayerMask obstacleMask;

    [SerializeField] int targetIndex = 0;
    [SerializeField] Transform currentTarget = null;

    [SerializeField] List<GameObject> targetList = new List<GameObject>();

    SpecialWeaponMissileLauncher specialWeaponMissile;

    TargetingCollisionDetection targetingCollisionDetection;

    void Awake()
    {
        specialWeaponMissile = GetComponent<SpecialWeaponMissileLauncher>();

        targetingCollisionDetection = GameObject.Find("TargetingRadar").GetComponent<TargetingCollisionDetection>();

        if (targetingCollisionDetection != null)
        {
            targetingCollisionDetection.playerMissileTargeting = this;
        }
    }

    void OnEnable()
    {
        targetList.Clear();
        currentTarget = null;

        StartCoroutine(ResetTransform());
    }

    IEnumerator ResetTransform()
    {
        yield return 0;

        transform.position = transform.position + Vector3.zero;
    }
    
    public void CollisionDetected(Transform potentialTarget)
    {
        Transform target = potentialTarget;

        //Check to make sure target is not already in the list and it has the needed script
        if (!targetList.Contains(target.gameObject) && target.gameObject.GetComponent<EnemyObjectTargeting>() != null)
        {
            AddTarget(target.gameObject);
        }
    }

    public void CollisionExitDetected(Transform targetToRemove)
    {
        RemoveTarget(targetToRemove.gameObject);
    }

    //Add target to the list
    void AddTarget(GameObject _targetAdded)
    {
        targetList.Add(_targetAdded);
        EnemyInMissileRange(_targetAdded, true);
        //Designate this as the selected target for player
        if (currentTarget == null)
        {
            SelectingTarget(_targetAdded, true, targetList.IndexOf(_targetAdded));  //Target our new enemy, IndexOf gets the index of the enemy in the targetList
        }

    }

    //Remove target from the list
    public void RemoveTarget(GameObject _targetRemoved)
    {
        targetList.Remove(_targetRemoved.gameObject);
        EnemyInMissileRange(_targetRemoved.gameObject, false);

        if (currentTarget == _targetRemoved.transform)
        {
            TargetRemoved();
        }
    }

    //Player switching between multiple targets and checking line of sight
    void Update()
    {
        int previousTarget = targetIndex;
        if (!PauseMenu.gameIsPaused && GameController.gameIsActive)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (targetIndex <= 0)
                {
                    targetIndex = targetList.Count - 1;
                }
                else targetIndex--;

                if (previousTarget != targetIndex)
                {
                    SwitchToNewTarget(targetIndex);
                }
            }

            if (currentTarget == null)
            {
                TargetRemoved();
            }

            CheckLineOfSightToAllEnemiesInRange();
        }
    }

    void CheckLineOfSightToAllEnemiesInRange()
    {
        foreach(GameObject enemy in targetList)
        {
            Transform target = enemy.transform;
            bool hasLineOfSight = false;

            //This Raycast detection code is adapted from Sebastain Lague's tutorial (though he used a sphere collider)
            //Direction and distance for the raycast, used to check if any walls or obstacles are between the player and the enemy
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            float distanceToTarget = Vector3.Distance(transform.position, target.position);

            //Check if no obstacles are in our line of sight
            if (!Physics2D.Raycast(transform.position, dirToTarget, distanceToTarget, obstacleMask))
            {
                hasLineOfSight = true;
            }

            EnemyObjectTargeting enemyInQuestion = target.GetComponent<EnemyObjectTargeting>();

            if (enemyInQuestion != null)
            {
                enemyInQuestion.HasLineOfSight(hasLineOfSight);
            }
        }
    }

    //Enemy is within player's range (enemy displays a target box)
    void EnemyInMissileRange(GameObject _enemyInQuestion, bool _lockedOn)
    {
        EnemyObjectTargeting enemy = _enemyInQuestion.GetComponent<EnemyObjectTargeting>();

        if (enemy != null)
        {
            enemy.InMissileRange(_lockedOn);
        }
    }

    //Switching to a new target
    void SwitchToNewTarget(int _newTargetIndex)
    {
        int i = 0;
        foreach (GameObject enemy in targetList)
        {
            if (i == _newTargetIndex)
            {
                SelectingTarget(enemy, true, _newTargetIndex);
            }
            else
            {
                SelectingTarget(enemy, false, targetIndex);
            }

            i++;
        }
    }

    //We've picked a new target
    void SelectingTarget(GameObject _enemyInQuestion, bool targeted, int _newTargetIndex)
    {
        EnemyObjectTargeting enemyObjectTargeting = _enemyInQuestion.GetComponent<EnemyObjectTargeting>();

        enemyObjectTargeting.Targeted(targeted);

        if (targeted)
        {
            currentTarget = enemyObjectTargeting.transform;
            targetIndex = _newTargetIndex;
        }
    }

    //Check if enemy is actually locked on before firing
    public void EnemyLockedOn(GameObject lockedOnEnemy, bool _isLockedOn)
    {
        //Set the target for the player's homing missiles
        currentTarget = lockedOnEnemy.transform;

        if (_isLockedOn)
        {
            //We've finally locked onto the enemy with our missile and are ready to fire
            specialWeaponMissile.NewTarget(currentTarget);
        }
        else
        {
            specialWeaponMissile.NewTarget(null);
        }
    }

    //Target is removed due to being destroyed or out of range
    void TargetRemoved()
    {
        if (gameObject.activeSelf)
        {
            StartCoroutine(SelectTargetAfterRemoval());
        }
    }

    IEnumerator SelectTargetAfterRemoval()
    {
        //We have a delay in case the enemy target was destroyed, so we have to wait till the end of frame before picking a new target
        yield return 0;

        currentTarget = null;
        specialWeaponMissile.NewTarget(currentTarget);
        targetIndex = targetList.Count - 1;
        SwitchToNewTarget(targetIndex);
    }
}
