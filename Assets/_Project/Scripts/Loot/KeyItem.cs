
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Key Item", menuName = "Item/Key Item")]
public class KeyItem : ItemObject
{
    //public ItemDatabase.SpecialItemIds specialItemId;

    public override ItemObject GetItem(){return this;}
    public override Blaster GetBlaster(){ return null; }
    public override Skill GetSkill(){return null;}
    public override Augment GetAugment(){return null;}
    public override KeyItem GetKeyItem(){return this;}
    public override Frame GetFrame() { return null; }
}
