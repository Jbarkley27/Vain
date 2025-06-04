using UnityEngine;

public class GlobalDataStore : MonoBehaviour
{
    public static GlobalDataStore Instance;


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


}