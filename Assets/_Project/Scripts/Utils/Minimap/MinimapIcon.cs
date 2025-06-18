using System.Collections.Generic;
using UnityEngine;

public class MinimapIcon : MonoBehaviour
{
    public enum MinimapIconType {PLANET, PLAYER, ENEMY_BASIC, ENEMY_ELITE, ENEMY_BOSS, POI, CHEST, HEALTH, LOOT, PORTAL};    
    public MinimapIconType _iconType;
    public GameObject iconVisual;

    public float minimapSize = .125f;
    public float fullScreenSize = 4f;

    void Start()
    {
        MiniMap.AddNonSpecialUIMarker(this);
    }

    void Update()
    {
        
    }
}
