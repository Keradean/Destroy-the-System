using UnityEngine;

public class RuntimeEnemyStats : MonoBehaviour
{
    private EnemyDataSO baseData;

    public float MaxHealth { get; private set; }
    public float MoveSpeed { get; private set; }
    public float AttackDamage { get; private set; }
    public float MeleeRange { get; private set; }
    public float RangedRange { get; private set; }
    public float SpecialRange { get; private set; }
    public float AttackCooldown { get; private set; }
    public float SpecialCooldown { get; private set; }
    public float XP { get; private set; }
    public bool IsSpecial { get; private set; }

    public void SetupStats(EnemyDataSO data, float stageMultiplier)
    {
        baseData = data;
        MaxHealth = data.baseHealth * stageMultiplier;
        AttackDamage = data.baseDamage * stageMultiplier;
        MeleeRange = data.meleeRange;
        RangedRange = data.rangedRange;
        SpecialRange = data.specialRange;
        AttackCooldown = data.attackCooldown;
        SpecialCooldown = data.specialCooldown;
        XP = data.xp * stageMultiplier;
        IsSpecial = data.hasSpecial;
    }
}