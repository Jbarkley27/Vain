public interface IAffix
{
    void OnEquip();
    void OnUnequip();
    void OnKill(EnemyBase enemy = null, Blaster blaster = null, Skill skill = null);
    void OnSkillUse(Skill skill);
    void Tick(); // for passive updates
    bool IsActive { get; }
}
