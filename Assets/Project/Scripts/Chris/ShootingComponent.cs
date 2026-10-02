using UnityEngine;

public class ShootingComponent : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;

    public void Fire(Transform target, float damage)
    {
        if (projectilePrefab || target == null) return;
        Vector3 dir = (target.position - firePoint.position).normalized;
       //  GameObject projectile = //TODO OPBJECTPOOLING you moron, in current project it is set not this one yet
        
    }
}
