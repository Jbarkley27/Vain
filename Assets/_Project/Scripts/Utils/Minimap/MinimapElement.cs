using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class MinimapElement : MonoBehaviour
{
    public Image icon;
    public CanvasGroupFlasher canvasGroupFlasher;
    public MiniMapKey.MinimapIconType minimapIconType;

    void Start()
    {
        MiniMap.minimapElements.Add(this);
    }



}