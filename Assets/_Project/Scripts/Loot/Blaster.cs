
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Blaster", menuName = "Item/Blaster")]
public class Blaster : ItemObject
{
    public float FireRate;
    public int MaxAmmo;
    public float CooldownRate;
    public int BaseDamage;
    public float RegenRate;
    public int CurrentAmmo;
    public GameObject ProjectilePrefab;
    public float Accuracy;
    public int BurstAmount;
    public float BurstTime;
    public int BurstAmountPerShot;
    public float BurstAmountPerShotFireRate;
    public float ProjectileSpeed;
    public float Range;
    public enum BlasterFireType {AUTO, MANUAL};
    public BlasterFireType FireType;
    public AnimationCurve animationCurve;
    public Sprite BlasterIcon;
    public ScreenShakeManager.ShakeProfile ScreenShakeProfile;


    private void Start()
    {
        // CurrentAmmo = MaxAmmo;
    }



    public override ItemObject GetItem() { return this; }
    public override Blaster GetBlaster() { return this; }
    public override Skill GetSkill() { return null; }
    public override Augment GetAugment() { return null; }
    public override KeyItem GetKeyItem() { return null; }
    public override Frame GetFrame() { return null; }
}
