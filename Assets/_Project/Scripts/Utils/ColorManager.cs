using UnityEngine;

public class ColorManager : MonoBehaviour
{
    public static ColorManager Instance;

    public Color plasmaPrimaryColor;
    public Color plasmaSecondaryColor;

    public Color scorchPrimaryColor;
    public Color scorchSecondaryColor;

    public Color joltPrimaryColor;
    public Color joltSecondaryColor;

    public Color corrosionPrimaryColor;
    public Color corrosionSecondaryColor;
    public Material normalHitFlashMat;


    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Found a Color Manager object, destroying new one");
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        // Debug.Log("PLayer Posi " + Player.transform.position);
    }

    public Color GetPrimaryColor(WeaponTypes.ElementType type)
    {
        switch (type)
        {
            case WeaponTypes.ElementType.PLASMA:
                return plasmaPrimaryColor;
            case WeaponTypes.ElementType.SCORCH:
                return scorchPrimaryColor;
            case WeaponTypes.ElementType.JOLT:
                return joltPrimaryColor;
            case WeaponTypes.ElementType.CORROSION:
                return corrosionPrimaryColor;
            default:
                Debug.LogError("Unknown Element Type: " + type);
                return Color.white; // Default color if unknown
        }
    }

    public Color GetSecondaryColor(WeaponTypes.ElementType type)
    {
        switch (type)
        {
            case WeaponTypes.ElementType.PLASMA:
                return plasmaSecondaryColor;
            case WeaponTypes.ElementType.SCORCH:
                return scorchSecondaryColor;
            case WeaponTypes.ElementType.JOLT:
                return joltSecondaryColor;
            case WeaponTypes.ElementType.CORROSION:
                return corrosionSecondaryColor;
            default:
                Debug.LogError("Unknown Element Type: " + type);
                return Color.white; // Default color if unknown
        }
    }
}

[CreateAssetMenu(fileName = "New Shader", menuName = "Ship Shader/Shader")]
public class ShaderData : ScriptableObject
{
    public Material primary;
    public Material secondary;
    public Material tertiary;
    public Material quaternary;
}