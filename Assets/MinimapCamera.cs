using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    public float height = 100f; // how high above the player
    public bool rotateWithPlayer = false;


    void LateUpdate()
    {
        Transform target = GlobalDataStore.Instance.Player.transform;
        if (target == null) return;

        Vector3 newPos = target.position;
        newPos.y += height;
        transform.position = newPos;

        // if (rotateWithPlayer)
        //     transform.rotation = Quaternion.Euler(90f, target.eulerAngles.y, 0f);
        // else
        //     transform.rotation = Quaternion.Euler(90f, 0f, 0f);
    }
}
