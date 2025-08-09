using UnityEngine;
using Random=UnityEngine.Random;

[RequireComponent(typeof(Rigidbody))]
public class PlayerProjectile : ProjectileBase
{
    void Update()
    {
        timeAlive += Time.deltaTime;

        float t = Mathf.Clamp01(timeAlive / lifetime);
        float currentSpeed = baseSpeed * speedCurve.Evaluate(t);

        // Update velocity based on the curve
        rb.linearVelocity = moveDirection * currentSpeed;

        if (timeAlive >= lifetime)
            Destroy(gameObject);
    }

    public float testForce;
    public float duration;

    public override void OnTriggerEnter(Collider collision)
    {
        // Debug.Log("Hit " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log($"Hit Enemy {collision.gameObject.name}");
            EnemyBase enemy = collision.gameObject.GetComponent<EnemyBase>();

            if (!enemy)
            {
                // Debug.Log(collision.gameObject.transform.root.gameObject.GetComponent<EnemyBase>());
                return;
            }


            enemy.TakeDamage(damage + StatManager.Instance.GetElementDamageValue(elementType), statusEffectType, 100);

            // Get direction from attacker to target
            Vector3 direction = transform.position - collision.transform.position;

            // Remove vertical component for horizontal knockback
            direction.y = 0f;

            // Normalize the result
            direction.Normalize();

            if(collision.gameObject) collision.gameObject.GetComponent<KnockbackReceiver>().ApplyKnockback(direction, Random.Range(knockbackForce, knockbackForce + 70f), duration);
            Destroy(gameObject);
            // }
        }
        // else
        // {
        //     Destroy(gameObject);
        // }
    }
}
