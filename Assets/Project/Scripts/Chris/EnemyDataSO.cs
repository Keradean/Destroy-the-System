using UnityEngine;

public enum EnemyType
{
    Scanner,
    Tracer,
    Firewall,
    Administrator
}

[CreateAssetMenu(menuName = "Entities/EnemyDataSO")]
public class EnemyDataSO : ScriptableObject
{
    [Header("Identity")]
    public string enemyName;
    public EnemyType type;
    public GameObject prefab;

    [Header("Base Stats")]
    public float xp;
    public float baseHealth;
    public float baseDamage;
    public float meleeRange;
    public float rangedRange;

    [Header("Special Stats")]
    public bool hasSpecial;
    public float specialRange;
    public float specialCooldown;
}