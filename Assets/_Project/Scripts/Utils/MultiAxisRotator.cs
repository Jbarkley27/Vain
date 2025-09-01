using UnityEngine;

public class MultiAxisRotator : MonoBehaviour
{
    [SerializeField] private float minSpeed = 30f;  // min degrees/sec per axis
    [SerializeField] private float maxSpeed = 90f;  // max degrees/sec per axis

    private Vector3 rotationSpeeds;

    void Start()
    {
        // Pick a random speed for each axis
        rotationSpeeds = new Vector3(
            Random.Range(minSpeed, maxSpeed),
            Random.Range(minSpeed, maxSpeed),
            Random.Range(minSpeed, maxSpeed)
        );

        // Randomly flip directions
        rotationSpeeds.x *= Random.value > 0.5f ? 1f : -1f;
        rotationSpeeds.y *= Random.value > 0.5f ? 1f : -1f;
        rotationSpeeds.z *= Random.value > 0.5f ? 1f : -1f;
    }

    void Update()
    {
        // Apply continuous rotation on all axes
        transform.Rotate(rotationSpeeds * Time.deltaTime);
    }
}
