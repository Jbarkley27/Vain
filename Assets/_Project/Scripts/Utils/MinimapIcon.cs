using System.Collections.Generic;
using UnityEngine;

public class MinimapIcon : MonoBehaviour
{
    // public Transform target; // 3D world object to follow
    // public RectTransform uiIcon; // UI icon on canvas
    // public Canvas canvas;
    // public Camera mainCamera;
    public enum MinimapIconType {PLANET, PLAYER, ENEMY_BASIC, ENEMY_ELITE, ENEMY_BOSS, POI, CHEST, HEALTH, LOOT};    
    public MinimapIconType _iconType;
    public GameObject iconVisual;

    void Start()
    {
        // MiniMap.allUIMarkers.Add(this);
    }

    void Update()
    {
        // if (target == null || uiIcon == null) return;

        // // Convert world position to screen point
        // Vector3 screenPos = mainCamera.WorldToScreenPoint(target.position);

        // uiIcon.gameObject.SetActive(true);

        // // Set UI icon position
        // Vector2 anchoredPos;
        // RectTransformUtility.ScreenPointToLocalPointInRectangle(
        //     canvas.transform as RectTransform,
        //     screenPos,
        //     canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : mainCamera,
        //     out anchoredPos
        // );

        // uiIcon.anchoredPosition = anchoredPos;
    }
}
