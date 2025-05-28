using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class EnemyPrefabEntry {
    public EnemyID ID;
    public GameObject Prefab;
}

public class EnemyDatabase : MonoBehaviour {
    public List<EnemyPrefabEntry> Entries;

    private Dictionary<EnemyID, GameObject> _lookup;

    void Awake() {
        _lookup = Entries.ToDictionary(e => e.ID, e => e.Prefab);
    }

    void Start()
    {
        Debug.Log(GetPrefab(EnemyID.Sniper).name);
    }

    public GameObject GetPrefab(EnemyID id)
    {
        if (_lookup.TryGetValue(id, out var prefab))
        {
            return prefab;
        }

        Debug.LogError($"[EnemyLibrary] Missing prefab for EnemyID: {id}");
        return null;
    }
}
