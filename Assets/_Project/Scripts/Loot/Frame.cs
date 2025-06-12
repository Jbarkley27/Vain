using UnityEngine;
using System.Collections;
using UnityEditor.Experimental.GraphView;

[CreateAssetMenu(fileName = "New Frame", menuName = "Item/Frame")]
public class Frame : ItemObject
{
    public GameObject frame;
    // public ItemDatabase.FrameIds frameId;

    public override ItemObject GetItem() { return this; }
    public override Blaster GetBlaster() { return null; }
    public override Skill GetSkill() { return null; }
    public override Resource GetResource() { return null; }
    public override Augment GetAugment() { return null; }
    public override KeyItem GetKeyItem() { return null; }
    public override Frame GetFrame() { return this; }
}

