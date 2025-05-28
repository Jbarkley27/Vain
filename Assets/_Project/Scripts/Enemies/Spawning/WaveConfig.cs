using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyWaveEntry {
    public EnemyID enemyID;
    public int count;
    public int tier; // Ties to world difficulty system
}

[CreateAssetMenu(fileName = "NewWaveConfig", menuName = "Spawning/Wave Config")]
public class WaveConfig : ScriptableObject {
    public List<EnemyWaveEntry> enemies;
}
