using UnityEngine;

public class EnemyRaycaster : MonoBehaviour
{
    public int rayCount = 12;
    public float rayLength = 10f;
    public LayerMask hitLayers;
    public Gradient normalColor;
    public Gradient hitColor;
    public float hitForce = 10f;
    private LineRenderer[] lineRenderers;

    void Start()
    {
        // Setup LineRenderers
        lineRenderers = new LineRenderer[rayCount];
        for (int i = 0; i < rayCount; i++)
        {
            GameObject lrObj = new GameObject("Ray_" + i);
            lrObj.transform.parent = transform;
            LineRenderer lr = lrObj.AddComponent<LineRenderer>();

            lr.positionCount = 2;
            lr.startWidth = 0.4f;
            lr.endWidth = 0.4f;
            lr.material = new Material(Shader.Find("Sprites/Default")); // simple shader
            lr.colorGradient = normalColor;

            lineRenderers[i] = lr;
        }

        rayLength = Random.Range(rayLength, rayLength * 1.5f);
    }

    void Update()
    {
        if (!EnemyManager.Instance.ShowEnemyDebugRays)
        {
            foreach (var lr in lineRenderers)
            {
                lr.enabled = false;
            }
            return;
        }
        else
        {
            foreach (var lr in lineRenderers)
            {
                lr.enabled = true;
            }
        }

        float angleStep = 360f / rayCount;

        for (int i = 0; i < rayCount; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;

            // Direction in XZ plane
            Vector3 dir = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle));

            // Ray start & end
            Vector3 start = transform.position;
            Vector3 end = start + dir * rayLength;

            if (Physics.Raycast(start, dir, out RaycastHit hit, rayLength, hitLayers))
            {
                // Update line to stop at hit point
                end = hit.point;
                lineRenderers[i].colorGradient = hitColor;

                // Apply force (if object has rigidbody)
                Rigidbody rb = hit.collider.attachedRigidbody;
                if (rb != null)
                {
                    Debug.Log($"Ray {i} hit {hit.collider.name} at distance {hit.distance} applying force {dir * hitForce} + Current Velocity {rb.linearVelocity}");
                    // lets make it so that the force amount is based on how close the hit is
                    float distanceFactor = 1f - (hit.distance / rayLength) * hitForce;
                    distanceFactor = Mathf.Clamp(distanceFactor, 0f, hitForce);
                    rb.AddForce(dir * distanceFactor, ForceMode.VelocityChange);
                }
            }
            else
            {
                lineRenderers[i].colorGradient = normalColor;
            }

            // Update line positions
            lineRenderers[i].SetPosition(0, start);
            lineRenderers[i].SetPosition(1, end);
        }
    }
}
