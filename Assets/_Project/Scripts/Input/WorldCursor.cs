using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class WorldCursor : MonoBehaviour
{
    public static WorldCursor instance;
    
    [SerializeField] private LayerMask _cursorLayer;
    [SerializeField] private RectTransform _cursorUI;
    [SerializeField] private InputManager _inputManager;
    [SerializeField] private GameObject _physicalWorldCursor;
    [SerializeField] private float _cursorSpeed = 10;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found a Cursor Manager object, destroying new one");
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }


    void Update()
    {
        Cursor.visible = false;
        CursorUI();
    }


    public void CursorUI()
    {
        Vector3 cursorInput = _inputManager.CursorInput;

        if (_inputManager.CurrentDevice == InputManager.InputDevice.K_M)
        {
            // MOUSE INPUT

            // clamp the cursor to the screen bounds
            cursorInput.x = Mathf.Clamp(cursorInput.x, 0, Screen.width);
            cursorInput.y = Mathf.Clamp(cursorInput.y, 0, Screen.height);

            _cursorUI.position = cursorInput;
        }
        else if (_inputManager.CurrentDevice == InputManager.InputDevice.GAMEPAD)
        {
            // GAMEPAD INPUT

            // make sure the cursor doesn't go off screen
            Vector3 cursorPos = _cursorUI.position;

            cursorPos.x += cursorInput.x * _cursorSpeed;
            cursorPos.y += cursorInput.y * _cursorSpeed;

            // clamp the cursor to the screen bounds
            cursorPos.x = Mathf.Clamp(cursorPos.x, 0, Screen.width);
            cursorPos.y = Mathf.Clamp(cursorPos.y, 0, Screen.height);

            _cursorUI.position = cursorPos;
        }

        MovePhysicalCursor();
    }

    public void MovePhysicalCursor()
    {
        if (Camera.main == null || _physicalWorldCursor == null)
        {
            Debug.LogWarning("Camera.main or physicalWorldCursor is null");
            return;
        }
        // raycast to get the position of the line end
        Ray ray = Camera.main.ScreenPointToRay(_cursorUI.GetComponent<RectTransform>().position);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, _cursorLayer))
        {
            Vector3 nextPos = new Vector3(hit.point.x, 0, hit.point.z);
            _physicalWorldCursor.transform.position = nextPos;
        }
    }

      public Vector3 GetDirectionFromWorldCursor(Vector3 source)
    {
        if (_physicalWorldCursor == null)
        {
            Debug.LogError("physicalWorldCursor is null");
            return Vector3.zero;
        }

        return _physicalWorldCursor.transform.position - source;
    }
}
