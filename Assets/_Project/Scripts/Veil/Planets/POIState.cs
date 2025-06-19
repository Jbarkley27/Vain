using UnityEngine;

public class POIState : MonoBehaviour
{
    public bool IsTaken = false;

    void Start()
    {
        IsTaken = false;
    }

    public void SetTakenStatus(bool status)
    {
        IsTaken = status;
    }
}