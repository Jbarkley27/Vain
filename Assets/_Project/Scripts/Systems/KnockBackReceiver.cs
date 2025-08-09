using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class KnockbackReceiver : MonoBehaviour
{
    [Header("References")]
    public Rigidbody rb;
    public NavMeshAgent agent;

    [Header("Knockback Settings")]
    public float knockbackForce = 10f;
    public float knockbackDuration = 0.3f;

    private bool isKnockedBack = false;

    private void Reset()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
    }

    public void ApplyKnockback(Vector3 direction, float force = -1f, float duration = -1f)
    {
        if (isKnockedBack || !agent.gameObject.activeSelf) return;

        float finalForce = (force > 0) ? force : knockbackForce;
        float finalDuration = (duration > 0) ? duration : knockbackDuration;

        StartCoroutine(KnockbackCoroutine(direction.normalized, finalForce, finalDuration));
    }

    private IEnumerator KnockbackCoroutine(Vector3 direction, float force, float duration)
    {
        isKnockedBack = true;

        // Disable AI and enable physics
        agent.enabled = false;
        // rb.isKinematic = false;
        rb.linearVelocity = Vector3.zero;

        // Apply knockback
        rb.AddForce(direction * force, ForceMode.Impulse);

        // Wait during knockback
        yield return new WaitForSeconds(duration);

        // Stop motion, re-enable AI
        rb.linearVelocity = Vector3.zero;
        // rb.isKinematic = true;
        agent.enabled = true;

        isKnockedBack = false;
    }
}