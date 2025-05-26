using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerMovement : MonoBehaviour
{

    [Header("General")]
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _maxVelocity;
    private InputManager _inputManager;


    [Header("Rotate Settings")]
    [SerializeField] private float _rotateSpeed = 1.0f;
    private float _rotateDirection;
    private float _rotateDifference;


    [Header("Boost")]
    [SerializeField] private float _boostForce;


    [Header("Dash Settings")]
    public bool CanDash = true;
    [SerializeField] float _dashDrag;
    private float _defaultDrag;
    [SerializeField] float _resetMomentumTime;
    [SerializeField] float _dashForce;
    public bool IsDashing = false;
    [SerializeField] float _dashForHowLong;
    [SerializeField] private float _dashCooldown;
    // private List<MeshRenderer> meshesToHideWhenDashing = new List<MeshRenderer>();
    // public GameObject playerMesh;


    [Header("Thrust Settings")]
    [SerializeField] private float _forceMagnitude = 5.55f;
    [SerializeField] private float _xForce;
    [SerializeField] private float _yForce;


    [Header("Animation")]
    [SerializeField] private Animator _animator;
    [SerializeField] private float _dampTime;



    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _inputManager = GetComponent<InputManager>();
        _defaultDrag = _rb.linearDamping;
    }

    void Update()
    {
        HandleAnimations();
    }

    private void FixedUpdate()
    {
        // ensure player y position is always 0
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        RotateTowards(WorldCursor.instance.GetDirectionFromWorldCursor(transform.position));
        Thrust();
    }



    // ROTATION HANDLING -----------------------------------------------------
    private void RotateTowards(Vector3 targetDirection)
    {
        if (targetDirection == Vector3.zero)
            return;


        // Calculate the target rotation
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

        targetRotation = targetRotation.normalized;

        // only rotate around the y axis
        targetRotation.x = 0;
        targetRotation.z = 0;


        // Smoothly interpolate between current and target rotation
        Quaternion smoothedRotation = Quaternion.Slerp(
            _rb.rotation,                              // Current rotation
            targetRotation,                            // Target rotation
            _rotateSpeed * Time.deltaTime              // Interpolation factor
        );

        // used for animation
        _rotateDifference = Quaternion.Angle(gameObject.transform.rotation, targetRotation);
        _rotateDirection = Vector3.Dot(targetDirection, transform.right);


        smoothedRotation = smoothedRotation.normalized;

        // Apply the smooth rotation to the Rigidbody
        _rb.MoveRotation(smoothedRotation);
    }

    // THRUST HANDLING ------------------------------------------------------
    private void Thrust()
    {
        if (_inputManager.ThrustInput.magnitude == 0) return;

        float xThrustInput = _inputManager.ThrustInput.x;
        float yThrustInput = _inputManager.ThrustInput.y;

        // Project camera-relative directions onto horizontal plane
        // Get camera-relative directions
        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;

        // Flatten the vectors to the XZ plane
        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 movementDirectionX = camRight * xThrustInput * _xForce * _forceMagnitude;
        Vector3 movementDirectionY = camForward * yThrustInput * _yForce * _forceMagnitude;

        _rb.AddForce(movementDirectionX, ForceMode.Force);
        _rb.AddForce(movementDirectionY, ForceMode.Force);
    }


    // BOOST HANDLING ------------------------------------------------------
    private void Boost()
    {
        if (!_inputManager.IsBoosting) return;

        _rb.AddForce(gameObject.transform.forward * _boostForce, ForceMode.Force);
    }




    public IEnumerator Dash()
    {

        if (!CanDash || IsDashing) yield return null;

        Debug.Log("Dashing");

        // Disable further dashes during cooldown
        CanDash = false;
        IsDashing = true;

        // Apply a force in the direction of thrust input
        var finalDir = new Vector3(_inputManager.ThrustInput.x, 0, _inputManager.ThrustInput.y);


        _rb.AddRelativeForce(finalDir * _dashForce, ForceMode.VelocityChange);


        // Hide Player while dashing
        // foreach (MeshRenderer mr in meshesToHideWhenDashing) mr.enabled = false;


        // Wait for the dash time
        yield return new WaitForSeconds(_dashForHowLong);

        // Change Drag
        _rb.linearDamping = _dashDrag;

        // THIS IS IMPORTANT - It adds the needed drag to get the dash stop pause feeling just right
        yield return new WaitForSeconds(_resetMomentumTime);

        // Unhide Player
        // foreach (MeshRenderer mr in meshesToHideWhenDashing) mr.enabled = true;

        _rb.linearDamping = _defaultDrag;
        IsDashing = false;

        yield return new WaitForSeconds(_dashCooldown);

        CanDash = true;
    }
    
    // ANIMATION HANDLING ---------------------------------------------------
    public void HandleAnimations()
    {
        if (_animator == null)
            return;

        float finalRollAmount = _rotateDifference;

        if (_rotateDirection < 0)
        {
            finalRollAmount *= -1;
        }

        float scaledRoll = ScaleValue(finalRollAmount);

        _animator.SetFloat("RotateDifference", scaledRoll, _dampTime, Time.deltaTime);
    }

    public float ScaleValue(float value)
    {
        float min = -45f;
        float max = 45f;

        // Ensure the value is clamped within the original range
        value = Mathf.Clamp(value, min, max);

        // Scale the value to the range -1 to 1
        return value / max; // Equivalent to (value - min) / (max - min) * 2 - 1
    }
}
