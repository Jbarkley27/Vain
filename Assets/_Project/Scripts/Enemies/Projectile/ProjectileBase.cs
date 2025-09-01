using DG.Tweening;
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
    public GameObject hitEffectPrefab;

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
        rb.AddForce(moveDirection * baseSpeed, ForceMode.Impulse);

        transform.DOScale(new Vector3(2, 2, 2), 1.5f).From(Vector3.zero).SetEase(Ease.OutBounce);
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

    // public virtual void OnCol(Collision collision)
    // {
    //     if (collision.gameObject.CompareTag("Player"))
    //     {
    //         Debug.Log("Hit Player");
    //         PlayerHealth.Instance.TakeDamage(damage);
    //         // if (hitEffectPrefab != null)
    //         // {
    //         //     GameObject hitEffect = Instantiate(hitEffectPrefab, collision.transform.position, Quaternion.identity);
    //         //     Destroy(hitEffect, 1f);
    //         // }
    //         // make fake explosion using a sphere primitive
    //         GameObject tempExplosion = GameObject.CreatePrimitive(PrimitiveType.Sphere);
    //         tempExplosion.transform.position = 
    //         Destroy(gameObject);
    //     }
    // }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Projectile hit " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Hit Player");
            PlayerHealth.Instance.TakeDamage(damage);
            if (hitEffectPrefab != null)
            {
                GameObject hitEffect = Instantiate(hitEffectPrefab, collision.contacts[0].point, Quaternion.identity);
                hitEffect.transform.localScale = Vector3.one * Random.Range(0.9f, 1.3f);
                Destroy(hitEffect, 1.2f);
            }
            // make fake explosion using a sphere primitive
            // GameObject tempExplosion = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            // tempExplosion.transform.position = collision.contacts[0].point;
            // tempExplosion.transform.DOScale(new Vector3(2, 2, 2), Random.Range(.4f, .6f)).From(Vector3.zero).SetEase(Ease.Linear);

            // Destroy(tempExplosion, 5);

            // float scale = Random.Range(2, 3);
            // tempExplosion.transform.DOScale(new Vector3(scale, scale, scale), Random.Range(.4f, .6f)).From(Vector3.zero).SetEase(Ease.Linear)
            // .OnComplete(() => Destroy(tempExplosion));
            Destroy(gameObject);
        }
        else
        {
            // Optionally handle other collisions (e.g., walls) here
            // Destroy(gameObject);
        }
    }
}
