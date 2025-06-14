using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemObject : ScriptableObject
{
    [Header("Base Item Fields")]
    public string itemName;
    public string description;
    public Sprite icon;


    public abstract ItemObject GetItem();
    public abstract Blaster GetBlaster();
    public abstract Skill GetSkill();
    public abstract Augment GetAugment();
    public abstract KeyItem GetKeyItem();
    public abstract Frame GetFrame();
}
