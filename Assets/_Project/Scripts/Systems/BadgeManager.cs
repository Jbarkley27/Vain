using UnityEngine;
using UnityEngine.UI;



public class BadgeManager : MonoBehaviour
{
    public static BadgeManager Instance;
    public Badge CurrentBadge;
    public Image Icon;
    public Image Background;


    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Found a Weapon System object, destroying new one.");
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        if (CurrentBadge) CurrentBadge.affix.OnEquip();
    }

    public void EquipNewBadge(Badge newBadge)
    {
        if (!newBadge) return;
        newBadge.affix.OnUnequip();
        CurrentBadge = newBadge;
        Icon.sprite = newBadge.Icon;
        Background.color = newBadge.BackgroundColor;
        CurrentBadge.affix.OnEquip();
    }
}



[CreateAssetMenu(fileName = "New Badge", menuName = "Badge/Badge")]
public class Badge : ScriptableObject
{
    public AffixBase affix;
    public string Name;
    public Sprite Icon;
    public CanvasGroup canvasGroup;
    public Color BackgroundColor;
}