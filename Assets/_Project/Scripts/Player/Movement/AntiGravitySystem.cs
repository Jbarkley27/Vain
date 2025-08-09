using UnityEngine;
using TMPro;

public class AntiGravitySystem : MonoBehaviour
{
    public float hoverHeight = 2.0f;           // Desired hover height
    public float hoverForce = 5.0f;            // Base force applied to stay afloat
    public LayerMask groundLayer;              // To detect what counts as "ground"
    public Transform visualGO;
    private Rigidbody rb;
    public TMP_Text groundedStatusText;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Ray ray = new Ray(visualGO.position, -visualGO.up);
        RaycastHit hit;

        Debug.DrawLine(visualGO.position, -visualGO.up * hoverHeight, Color.red);

        if (Physics.Raycast(ray, out hit, hoverHeight, groundLayer))
        {
            groundedStatusText.text = "Grounded";

            float distanceToGround = hit.distance;
            float heightDifference = hoverHeight - distanceToGround;
            float liftForce = heightDifference * hoverForce;

            rb.AddForce(visualGO.up * liftForce, ForceMode.Force);
        }
        else
        {
            groundedStatusText.text = "Not Grounded";
        }
    }
}
