using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Resource", menuName = "Item/Resource")]
public class Resource : ItemObject
{
    public Color color;
    // public ItemDatabase.ResourceIds resourceId;
    public int amountInInventory;

    public override ItemObject GetItem() { return this; }
    public override Blaster GetBlaster() { return null; }
    public override Skill GetSkill() { return null; }
    public override Resource GetResource() { return this; }
    public override Augment GetAugment() { return null; }
    public override KeyItem GetKeyItem() { return null; }
    public override Frame GetFrame() { return null; }
}
