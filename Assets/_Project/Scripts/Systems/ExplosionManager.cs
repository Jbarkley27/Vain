using System.Collections.Generic;
using UnityEngine;
using Random=UnityEngine.Random;

public class ExplosionManager : MonoBehaviour
{
    public static ExplosionManager Instance;
    public enum ExplosionType { SMALL };
    public List<GameObject> genericSmallExplosions = new List<GameObject>();
    public ScreenShakeManager.ShakeProfile smallExplosionScreenShakeProfile;
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Found a Inventory object, destroying new one");
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void CreateExplosion(Vector3 location, ExplosionType explosionType)
    {
        switch (explosionType)
        {
            case ExplosionType.SMALL:
                GameObject explosion = genericSmallExplosions[Random.Range(0, genericSmallExplosions.Count)];
                GameObject newExplosion = Instantiate(explosion, location, Quaternion.identity);
                ScreenShakeManager.Instance.DoShake(smallExplosionScreenShakeProfile);
                float randomDim = Random.Range(13f, 16f);
                newExplosion.transform.localScale = new Vector3(randomDim, randomDim, randomDim);
                break;

        }
    }
}
