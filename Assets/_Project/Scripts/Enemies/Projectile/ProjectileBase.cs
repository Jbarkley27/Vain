using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ProjectileBase : MonoBehaviour
{
    public Rigidbody rb;
    public Vector3 moveDirection;
    public float baseSpeed;
    public AnimationCurve speedCurve;
    public float lifetime;
    public float timeAlive;
    public int damage;
    public WeaponTypes.ElementType elementType;
    public StatusEffectBase.StatusEffectType statusEffectType;
    public float knockbackForce;

    public void Initialize(Vector3 direction, float speed, AnimationCurve curve, float life, int damage, float knockback = 0)
    {
        rb = GetComponent<Rigidbody>();
        moveDirection = direction;
        baseSpeed = speed;
        speedCurve = curve;
        lifetime = life;
        timeAlive = 0f;
        this.damage = damage;
        this.knockbackForce = knockback;

        // Initial velocity
        rb.linearVelocity = moveDirection * baseSpeed;

    }

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

    public virtual void OnTriggerEnter(Collider collision)
    {
        // Debug.Log("Hit " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Hit Player");
            PlayerHealth.Instance.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
