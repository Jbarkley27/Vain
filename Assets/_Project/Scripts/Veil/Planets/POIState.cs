using UnityEngine;

public class POIState : MonoBehaviour
{
    public bool IsTaken = false;
    public enum POIGrade
    {
        None,
        C,
        B,
        A,
    }

    public enum POIGradeCType
    {
        SMALL_HEALTH,
        SMALL_RESOURCE,
        ENVIRONMENT_ELEMENT,
        SHIP_SHADER,
    }

    public enum POIGradeBType
    {
        MEDIUM_HEALTH,
        MEDIUM_RESOURCE,
        SKILL_REWARD_1,
        SKILL_REWARD_3,
        PUZZLE,
        NPC,
        DUNGEON,
        ROAMING_MINI_BOSS,
        DERELICT_SMALL_SHIP,
        CHEST,
        UPGRADE
    }

    public enum POIGradeAType
    {
        DUNGEON,
        PUBLIC_EVENT,
        CLUSTER_OF_ENEMIES,
        DERELECT_LARGE_SHIP,
    }

    void Start()
    {
        IsTaken = false;
    }

    public void SetTakenStatus(bool status)
    {
        IsTaken = status;
    }
}