// using UnityEngine;

// public class CursorUIHoverCheck : MonoBehaviour
// {
//     public RectTransform uiImage1;
//     public MinimapElement minimapElement;


//     void Update()
//     {
//         if (AreRectTransformsOverlapping(uiImage1, WorldCursor.instance.targetUI))
//         {
//             if (minimapElement)
//             {
//                 minimapElement.cursorHovering = true;
//             }
//         }
//         else
//         {
//             if (minimapElement)
//             {
//                 minimapElement.cursorHovering = false;
//             }
//         }
//     }

//     bool AreRectTransformsOverlapping(RectTransform rect1, RectTransform rect2)
//     {
//         // Get the world corners of both RectTransforms
//         Vector3[] corners1 = new Vector3[4];
//         Vector3[] corners2 = new Vector3[4];
//         rect1.GetWorldCorners(corners1);
//         rect2.GetWorldCorners(corners2);

//         // Create Rect objects from the world corners
//         Rect r1 = new Rect(corners1[0].x, corners1[0].y, corners1[2].x - corners1[0].x, corners1[2].y - corners1[0].y);
//         Rect r2 = new Rect(corners2[0].x, corners2[0].y, corners2[2].x - corners2[0].x, corners2[2].y - corners2[0].y);

//         // Check for overlap
//         return r1.Overlaps(r2);
//     }
// }