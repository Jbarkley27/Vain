using UnityEngine;

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

            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
