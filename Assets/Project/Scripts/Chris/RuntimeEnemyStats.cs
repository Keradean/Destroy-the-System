using UnityEngine;

public class RuntimeEnemyStats : MonoBehaviour
{
    private EnemyDataSO baseData;

    public float MaxHealth { get; private set; }
    public float MoveSpeed { get; private set; }
    public float AttackDamage { get; private set; }
    public float AttackRange { get; private set; }

    public void SetupStats(EnemyDataSO data, float stageMultiplier)
    {
        baseData = data;
        MaxHealth = data.baseHealth * stageMultiplier;
        AttackDamage = data.baseDamage * stageMultiplier;
        AttackRange = data.attackRange;
    }
}