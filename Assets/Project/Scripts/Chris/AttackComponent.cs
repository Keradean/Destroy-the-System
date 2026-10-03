using UnityEngine;

[AddComponentMenu("Entities/Components/AttackComponent")]
public class AttackComponent : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Transform attackPoint;

    public Transform AttackPoint => attackPoint != null ? attackPoint : transform;

    public bool TryHitTarget(Transform target, float attackRange, float damage)
    {
        if (target == null) return false;
        float distSqr = (target.position - AttackPoint.position).sqrMagnitude;
        if (distSqr <= attackRange * attackRange)
        {
            if (target.TryGetComponent<HealthComponent>(out var heatlh))
            {
                heatlh.TakeDamage(damage);
                return true;
            }
        }
        return false;
    }
}
