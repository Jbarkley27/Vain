using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [Header("General")]
    private PlayerInput _playerInput;
    private PlayerMovement _playerMovement;
    public MiniMap _miniMap;

    [Header("Cursor")]
    public Vector2 CursorInput;
    public Vector3 CursorPosition;

    [Header("Thrust")]
    public Vector2 ThrustInput;

    [Header("Dash")]
    public bool DashPressed;

    [Header("Boosting")]
    public bool IsBoosting = false;

    [Header("Shooting")]
    public bool IsShooting = false;

    [Header("Confirm Prompt")]
    public bool ConfirmPromptPressed = false;


    [Header("Current Device Settings")]
    public InputDevice CurrentDevice;
    public enum InputDevice { K_M, GAMEPAD };


    private void Start()
    {
        CursorInput = new Vector2(0, 0);
        _playerInput = GetComponent<PlayerInput>();
        _playerMovement = GetComponent<PlayerMovement>();
    }


    void Update()
    {
        GetCurrentDevice();
        if (!ExtractionManager.Instance.IsExtracting()) Boost();
        if (!ExtractionManager.Instance.IsExtracting()) Shoot();
    }



    public void Cursor(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            CursorInput = context.ReadValue<Vector2>();
        }
    }

    public void Thrust(InputAction.CallbackContext context)
    {
        if (ExtractionManager.Instance.IsExtracting()) return;

        if (context.performed)
        {
            ThrustInput = context.ReadValue<Vector2>();
        }
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if (ExtractionManager.Instance.IsExtracting()) return;
        if (context.performed)
        {
            DashPressed = true;
            if (_playerMovement.CanDash) StartCoroutine(_playerMovement.Dash());
        }
        else if (context.canceled)
        {
            DashPressed = false;
        }
    }

    public void Boost()
    {

        if (_playerInput.actions["Boost"].IsPressed())
        {
            IsBoosting = true;
        }
        else if (_playerInput.actions["Boost"].WasReleasedThisFrame())
        {
            IsBoosting = false;
        }
    }

    public void OpenMinimap(InputAction.CallbackContext context)
    {
        if (ExtractionManager.Instance.IsExtracting()) return;
        if (context.performed)
        {
            _miniMap.ToggleMinimapScale();
        }
    }


    public void Shoot()
    {

        if (_playerInput.actions["Shoot"].IsPressed())
        {
            IsShooting = true;
        }
        else
        {
            IsShooting = false;
        }
    }


    public void UseSkill1(InputAction.CallbackContext context)
    {
        if (ExtractionManager.Instance.IsExtracting()) return;
        if (context.performed)
        {
            if (PlayerSkillManager.Instance != null)
            {
                PlayerSkillManager.Instance.UseSkill(0);
            }
            else
            {
                Debug.LogWarning("PlayerSkillManager instance is not available.");
            }
        }
    }


    public void UseSkill2(InputAction.CallbackContext context)
    {
        if (ExtractionManager.Instance.IsExtracting()) return;
        if (context.performed)
        {
            if (PlayerSkillManager.Instance != null)
            {
                PlayerSkillManager.Instance.UseSkill(1);
            }
            else
            {
                Debug.LogWarning("PlayerSkillManager instance is not available.");
            }
        }
    }


    public void ConfirmPrompt(InputAction.CallbackContext context)
    {
        if (ExtractionManager.Instance.IsExtracting()) return;
        if (context.performed)
        {
            ConfirmPromptPressed = true;
        }
        else if (context.canceled)
        {
            ConfirmPromptPressed = false;
        }
    }


    public void UseSkill3(InputAction.CallbackContext context)
    {
        if (ExtractionManager.Instance.IsExtracting()) return;
        if (context.performed)
        {
            if (PlayerSkillManager.Instance != null)
            {
                PlayerSkillManager.Instance.UseSkill(2);
            }
            else
            {
                Debug.LogWarning("PlayerSkillManager instance is not available.");
            }
        }
    }



    public void GetCurrentDevice()
    {
        if (_playerInput.currentControlScheme == "K&M")
        {
            CurrentDevice = InputDevice.K_M;
        }
        else if (_playerInput.currentControlScheme == "Gamepad")
        {
            CurrentDevice = InputDevice.GAMEPAD;
        }
    }

}
