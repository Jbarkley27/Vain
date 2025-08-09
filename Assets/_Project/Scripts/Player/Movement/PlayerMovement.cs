using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.VFX;



public class PlayerMovement : MonoBehaviour
{

    [Header("General")]
    [SerializeField] private Rigidbody _rb;
    private InputManager _inputManager;


    [Header("Rotate Settings")]
    [SerializeField] private float _rotateSpeed = 1.0f;
    private float _rotateDirection;
    private float _rotateDifference;



    [Header("Boost")]
    public BoostUI boostUI;
    public float boostAddition = 60f;


    [Header("Dash Settings")]
    public bool CanDash = true;

    // How tight the dash feels, adds resistance. Lower values makes it feel floaty
    [SerializeField] float _dashDrag;
    private float _defaultDrag;



    // How fast the dashDrag goes back to defaultDrag. Higher values makes the change of drags more noticeable.
    // Should keep this low ( current sweet spot = .1f)
    [SerializeField] float _resetMomentumTime;
    [SerializeField] float _dashForce;
    public bool IsDashing = false;
    // Controls the burst duration. Higher values will make the dash longer.
    [SerializeField] float _dashForHowLong;
    private List<MeshRenderer> meshesToHideWhenDashing = new List<MeshRenderer>();
    public GameObject playerMesh;
    public DashSkillUI dashSkillUI;



    [Header("Thrust Settings")]
    [SerializeField] private float _xForce;
    [SerializeField] private float _yForce;



    [Header("Animation")]
    [SerializeField] private Animator _animator;
    [SerializeField] private float _dampTime;

    [Header("VFX")]
    public VisualEffect speedLines;



    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _inputManager = GetComponent<InputManager>();
        _defaultDrag = _rb.linearDamping;
        meshesToHideWhenDashing = playerMesh.transform.GetComponentsInChildren<MeshRenderer>().ToList();
    }


    void Update()
    {
        HandleAnimations();
        HandleSpeedVFX();
    }


    private void FixedUpdate()
    {
        RotateTowards(WorldCursor.instance.GetDirectionFromWorldCursor(transform.position));
        Thrust();
        Boost();
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
        if (_inputManager.ThrustInput.magnitude == 0 || _inputManager.IsBoosting) return;

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

        Vector3 movementDirectionX = camRight * xThrustInput * _xForce * StatManager.Instance.GetThrustValue();
        Vector3 movementDirectionY = camForward * yThrustInput * _yForce * StatManager.Instance.GetThrustValue();

        _rb.AddForce(movementDirectionX, ForceMode.Force);
        _rb.AddForce(movementDirectionY, ForceMode.Force);
    }




    // BOOST HANDLING ------------------------------------------------------
    private void Boost()
    {
        if (!_inputManager.IsBoosting) return;
        _rb.AddForce(gameObject.transform.forward * (StatManager.Instance.GetBoostValue() + (boostUI.EnoughEnergyToBoost() ? boostAddition : 0f)), ForceMode.Impulse);
    }






    // DASH HANDLING ---------------------------------------------------------

    public void StartDash(float dashCooldown)
    {
        if (!CanDash || IsDashing) return;

        if (_inputManager.ThrustInput.magnitude == 0) return;

        // Disable further dashes during cooldown
        CanDash = false;
        IsDashing = true;

        StartCoroutine(Dash(dashCooldown));
    }


    public IEnumerator Dash(float dashCooldown)
    {
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

        Vector3 movementDirectionX = camRight * xThrustInput;
        Vector3 movementDirectionY = camForward * yThrustInput;

        Debug.Log("Dashing");

        _rb.AddForce(movementDirectionX * _dashForce, ForceMode.VelocityChange);
        _rb.AddForce(movementDirectionY * _dashForce, ForceMode.VelocityChange);
        StartCoroutine(
                dashSkillUI.UseSkill());

        // Hide Player while dashing
        foreach (MeshRenderer mr in meshesToHideWhenDashing) mr.enabled = false;


        // Wait for the dash time
        yield return new WaitForSeconds(_dashForHowLong);

        // Change Drag
        _rb.linearDamping = _dashDrag;

        // THIS IS IMPORTANT - It adds the needed drag to get the dash stop pause feeling just right
        yield return new WaitForSeconds(_resetMomentumTime);

        // Unhide Player
        foreach (MeshRenderer mr in meshesToHideWhenDashing) mr.enabled = true;

        _rb.linearDamping = _defaultDrag;
        IsDashing = false;
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



    // VFX -----------------------------------------------------------------------
    public void HandleSpeedVFX()
    {
        if (!_inputManager.IsBoosting)
        {
            speedLines.gameObject.SetActive(false);
            return;
        }

        speedLines.gameObject.SetActive(true);
        speedLines.SetBool("IsBoosting", boostUI.EnoughEnergyToBoost());
    }
}
