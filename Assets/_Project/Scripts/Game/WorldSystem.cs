using UnityEngine;

public class WorldSystem : MonoBehaviour
{
    public static WorldSystem Instance;


    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Found a Cursor Manager object, destroying new one");
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public float GetStatMultiplier()
    {
        return 1f;
    }
}