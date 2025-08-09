using System;
using UnityEngine;

public abstract class AffixBase : ScriptableObject, IAffix
{

    public virtual bool IsActive => true;

    public virtual void OnEquip()
    {
        throw new NotImplementedException();
    }

    public virtual void OnKill(EnemyBase enemy = null, Blaster blaster = null, Skill skill = null)
    {
        throw new NotImplementedException();
    }

    public void OnSkillUse(Skill skill)
    {
        throw new NotImplementedException();
    }

    public virtual void OnUnequip()
    {
        throw new NotImplementedException();
    }

    public void Tick()
    {
        throw new NotImplementedException();
    }
}
