using UnityEngine;

namespace Project.Scripts.Dennis.Player
{
    // Sucht den nächsten Gegner, startet die Attack-Animation und schießt beim Shoot-Event
    public class PlayerAttack : MonoBehaviour
    {
        private static readonly int AttackHash = Animator.StringToHash("Attack");

        [Header("Angriff")]
        [SerializeField] private float _attackInterval = 0.5f;      // Pause zwischen zwei Angriffen
        [SerializeField] private float _range = 5f;
        [SerializeField] private LayerMask _enemyLayer;
        [SerializeField] private float _maxAttackDuration = 3f;     // Sicherheitsnetz, falls AttackEnd verpasst wird

        [Header("Projektil")]
        [SerializeField] private GameObject _projectilePrefab;
        [SerializeField] private Vector3 _spawnOffset = new Vector3(0f, 0.5f, 0.5f);   // etwas höher und vor dem Player

        private readonly Collider[] _hits = new Collider[64];
        private Animator _animator;
        private Collider _target;
        private bool _isAttacking;
        private float _timer;

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
        }

        private void Update()
        {
            _timer += Time.deltaTime;

            if (_isAttacking)
            {
                if (_timer >= _maxAttackDuration) OnAttackFinished();
                return;
            }

            if (_timer < _attackInterval) return;

            Collider nearest = FindNearestEnemy();
            if (nearest == null) return;   // Bereitschaft, Timer läuft weiter

            _target = nearest;
            _isAttacking = true;
            _timer = 0f;

            Debug.Log("ANGRIFF startet auf " + nearest.name);   // TODO: nach dem Testen entfernen
            _animator.SetTrigger(AttackHash);
        }

        // Wird über AE_OnShootFrame aufgerufen
        public void Shoot()
        {
            if (!enabled) return;   // nach dem Tod nicht mehr schießen

            // Ziel könnte inzwischen tot oder weg sein, dann neu suchen
            if (!IsValidTarget(_target)) _target = FindNearestEnemy();
            if (_target == null) return;

            // Offset gilt relativ zum Player, z vorne heißt also immer in Blickrichtung
            Vector3 spawnPosition = transform.TransformPoint(_spawnOffset);
            GameObject projectile = Instantiate(_projectilePrefab, spawnPosition, Quaternion.identity);
            projectile.GetComponent<PlayerProjectile>().Launch(_target);
        }

        // Wird über AE_OnAttackEndFrame aufgerufen
        public void OnAttackFinished()
        {
            _isAttacking = false;
            _target = null;
            _timer = 0f;
        }

        private Collider FindNearestEnemy()
        {
            int hitCount = Physics.OverlapSphereNonAlloc(transform.position, _range, _hits, _enemyLayer);
            Collider nearest = null;
            float nearestDistance = Mathf.Infinity;

            for (int i = 0; i < hitCount; i++)
            {
                if (!IsValidTarget(_hits[i])) continue;

                float distance = Vector3.Distance(transform.position, _hits[i].transform.position);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearest = _hits[i];
                }
            }
            return nearest;
        }

        private static bool IsValidTarget(Collider target)
        {
            return target != null && target.enabled && target.gameObject.activeInHierarchy;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _range);
        }
    }
}