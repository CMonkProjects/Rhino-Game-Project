using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollisionDetector : MonoBehaviour
{
    public int priority;

    //Delegates
    public delegate void OnEnemyCollision();
    public OnEnemyCollision onEnemyCollision;
    public delegate void OnCanMove();
    public OnCanMove onCanMove;

    // Start is called before the first frame update
    void Start()
    {
        priority = ScoreManager.instanceScoreManager.priority;
        ScoreManager.instanceScoreManager.priority++;
    }

    protected internal void OnTriggerEnter2D(Collider2D _otherEnemy)
    {
        if (_otherEnemy.tag == "Enemy")
        {
            EnemyCollisionDetector otherEnemy = _otherEnemy.transform.parent.GetComponent<EnemyCollisionDetector>();

            if (otherEnemy != null)
            {
                if (otherEnemy.priority < priority)
                {
                    //call delegate
                    if (onEnemyCollision != null)
                    {
                        onEnemyCollision();
                    }
                }
            }
        }
    }

    protected internal void OnTriggerExit2D(Collider2D _otherEnemy)
    {
        EnemyCollisionDetector otherEnemy = _otherEnemy.transform.parent.GetComponent<EnemyCollisionDetector>();

        if (otherEnemy != null)
        {
            if (otherEnemy.priority < priority)
            {
                if (onCanMove != null)
                {
                    onCanMove();
                }
            }
        }
    }
}
