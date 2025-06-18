using System.Collections.Generic;
using UnityEngine;
using Unity.Cinemachine;

public class BastionManager : MonoBehaviour
{
    public static BastionManager Instance;

    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Found a BastionManager object, destroying new one");
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }


}