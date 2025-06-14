using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Item/Skill")]
public abstract class Skill : ItemObject
{
    public float castTime;
    public float rechargeTime;
    public float baseDamage;
    public float baseCooldownRate;
    public Sprite skillIcon;
    public WeaponTypes.ElementType elementType;
    // public ItemDatabase.SkillIds skillId;

    public abstract void Execute();


    public override ItemObject GetItem() { return this; }
    public override Blaster GetBlaster() { return null; }
    public override Skill GetSkill() { return this; }
    public override Augment GetAugment() { return null; }
    public override KeyItem GetKeyItem() { return null; }
    public override Frame GetFrame() { return null; }
}
