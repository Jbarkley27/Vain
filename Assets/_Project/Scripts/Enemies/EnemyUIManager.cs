using UnityEngine;

public class EnemyUIManager : MonoBehaviour
{
    public static EnemyUIManager Instance;
    public Canvas enemyUICanvas;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Found a Enemy UI Manager object, destroying new one");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public Transform GetCanvas() { return enemyUICanvas.transform; }
}