using UnityEngine;

public class AOEDamage : MonoBehaviour
{
    [SerializeField] internal float damage = 1;

    [SerializeField] protected float damageRadius;

    void Start()
    {
        gameObject.transform.parent = null;
        Explode();
        Destroy(gameObject);
    }

    internal virtual void Explode()
    {
        //Check all enemies within the overlap circle and apply damage
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, damageRadius);

        foreach (Collider2D hit in colliders)
        {
            if (hit.tag == "Enemy")
            {
                EnemyHealth enemyHealth = hit.GetComponent<EnemyHealth>();

                if (enemyHealth)
                    enemyHealth.TakeDamage(damage);
            }

            if (hit.tag == "Destructable")
            {
                DestructableHealth destructableHealth = hit.GetComponent<DestructableHealth>();

                if (destructableHealth)
                    destructableHealth.TakeDamage(damage);
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(transform.position, damageRadius);
    }
}