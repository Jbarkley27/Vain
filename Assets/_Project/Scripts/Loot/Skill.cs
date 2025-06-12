using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Item/Skill")]
public class Skill : ItemObject
{
    public float castTime;
    public Color primaryColor;
    public Color secondaryColor;
    public float rechargeTime;
    public float baseDamage;
    public float baseCooldownRate;
    public bool isDamageSkill;
    // public ItemDatabase.SkillIds skillId;


    public override ItemObject GetItem() { return this; }
    public override Blaster GetBlaster() { return null; }
    public override Skill GetSkill() { return this; }
    public override Resource GetResource() { return null; }
    public override Augment GetAugment() { return null; }
    public override KeyItem GetKeyItem() { return null; }
    public override Frame GetFrame() { return null; }
}
