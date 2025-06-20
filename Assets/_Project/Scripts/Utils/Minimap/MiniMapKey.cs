using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class MiniMapKey : MonoBehaviour
{
    public RectTransform thisRect;
    public bool Hovered;
    public Color normalColor;
    public Color highlightedColor;
    public UnityEngine.UI.Image backgroundColor;

    public enum MinimapIconType {PLANET, PLAYER, ENEMY_BASIC, ENEMY_ELITE, ENEMY_BOSS, POI, CHEST, HEALTH, LOOT, PORTAL};
    public MinimapIconType minimapIconType;
    public GameObject iconVisual;

    public float minimapSize = .125f;
    public float fullScreenSize = 4f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        thisRect = GetComponent<RectTransform>();
        backgroundColor = GetComponent<UnityEngine.UI.Image>();
        MiniMap.AddMiniMapKey(this);
    }

    void Update()
    {
        if (UIUtils.RectTransformOverlaps(thisRect, GlobalDataStore.Instance.worldCursorUI))
        {
            if (Hovered == false)
            {
                // this is the first time its switching to hovered
                GlobalDataStore.Instance.MiniMap.UpdatePanelData(minimapIconType);
            }
            Hovered = true;
        }
        else
        {
            // if (Hovered)
            // {
            //     // this is the first time its switching to hovered
            //     GlobalDataStore.Instance.MiniMap.FlashMapElements();
            // }

            Hovered = false;
        }


        Color targetColor = Hovered ? highlightedColor : normalColor;
        backgroundColor.color = Color.Lerp(backgroundColor.color, targetColor, .4f);
    }




}
