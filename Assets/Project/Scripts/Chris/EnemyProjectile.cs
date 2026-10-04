using UnityEngine;


//TODO: Dennis mach bitte ein Projectile dass mit deinem Player funktioniert. Ich lasse das hier mit der Logik einfach mal da.


[RequireComponent(typeof(Collider))]
public class EnemyProjectile : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float speed = 20f;
    [SerializeField] private float maxLifeTime = 5f;
    [SerializeField] private LayerMask targetLayer;

    public float damage { get; private set; }
    private float currentLifetime;
    private PoolManager poolManager;
    private Collider collider;

    private void Awake()
    {
        collider = GetComponent<Collider>();
        collider.isTrigger = true;
    }
    private void OnEnable()
    {
        currentLifetime = 0f;
    }
    public void Initialize(float damage, PoolManager pool, LayerMask mask)
    {
        this.damage = damage;
        poolManager = pool;
        targetLayer = mask;
    }
    private void Update()
    {
        transform.Translate(Vector3.forward * (speed * Time.deltaTime));
        currentLifetime += Time.deltaTime;
        if (currentLifetime >= maxLifeTime)
        {
            Despawn();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & targetLayer) != 0)
        {
            if (other.TryGetComponent<HealthComponent>(out var health))
                health.TakeDamage(damage);
            Despawn();
        }
    }
    private void Despawn()
    {
        if (poolManager != null) poolManager.Despawn(gameObject);
        else gameObject.SetActive(false);
    }
}
