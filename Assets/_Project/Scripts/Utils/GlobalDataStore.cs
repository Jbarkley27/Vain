using Unity.Cinemachine;
using UnityEngine;

public class GlobalDataStore : MonoBehaviour
{
    public static GlobalDataStore Instance;
    public PlanetDetector PlanetDetector;
    public GameObject Player;
    public GameObject DebugObject;
    public InputManager InputManager;

    public CinemachineCamera VirtualCameraVeil;
    public CinemachineCamera VirtualCameraBastion;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Found a GlobalDataStore Manager object, destroying new one");
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        // Debug.Log("PLayer Posi " + Player.transform.position);
    }

    public CinemachineCamera CorrectCamera()
    {
        return VirtualCameraVeil;
    }
}