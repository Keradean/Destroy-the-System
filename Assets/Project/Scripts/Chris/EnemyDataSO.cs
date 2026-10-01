using UnityEngine;

public enum EnemyType
{
    Scanner,
    Tracer,
    Firewall,
    Administrator
}

public class EnemyDataSO : ScriptableObject
{
    [Header("Identity")]
    public string enemyName;
    public EnemyType type;
    public GameObject prefab;

    [Header("Base Stats")]
    public float baseHaealth;
    public float baseDamage;
    public float movespeed;
    public float attackRange;
    public float aggroRadius;

    [Header("Special")]
    public float damageReductionPercent = 0;
    public float optimalRangeDistance;
    public float maximumRangeDistance;
}