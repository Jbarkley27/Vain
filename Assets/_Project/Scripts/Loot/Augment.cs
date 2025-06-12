using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

[CreateAssetMenu(fileName = "New Augment", menuName = "Item/Augment")]
public class Augment : ItemObject
{
    // TODO: remove later 
    public bool healingAugment;
    // public ItemDatabase.AugmentIds augmentId;

    public override ItemObject GetItem() { return this; }
    public override Blaster GetBlaster() { return null; }
    public override Skill GetSkill() { return null; }
    public override Resource GetResource() { return null; }
    public override Augment GetAugment() { return this; }
    public override KeyItem GetKeyItem() { return null; }
    public override Frame GetFrame() { return null; }
}
