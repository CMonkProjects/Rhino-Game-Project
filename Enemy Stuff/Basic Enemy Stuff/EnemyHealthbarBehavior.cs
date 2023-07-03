using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthbarBehavior : MonoBehaviour
{
    public GameObject[] healthbar;

    Transform enemyToFollow;
    Vector3 offSet = new Vector3(0, 0.2f, 0);

    public float activeTimer;
    float timer;

    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    void LateUpdate()
    {
        if (enemyToFollow != null)
        {
            transform.position = enemyToFollow.position + offSet;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetHealth(float _health, float _shields, bool _resetTimer)
    {
        healthbar[0].transform.localScale = new Vector3(_health, 0.5f);
        healthbar[1].transform.localScale = new Vector3(_shields, 0.5f);

        if (_resetTimer)
        {
            timer = activeTimer;
            gameObject.SetActive(true);
        }
    }

    public void FollowEnemy(Transform _enemy)
    {
        enemyToFollow = _enemy;
    }
}