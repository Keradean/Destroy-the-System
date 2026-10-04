using Project.Scripts.Dennis.Audio;
using UnityEngine;

namespace Project.Scripts.Dennis.Player
{
    // Sucht den nächsten Gegner, startet die Attack-Animation und schießt beim Shoot-Event
    public class PlayerAttack : MonoBehaviour
    {
        private static readonly int AttackHash = Animator.StringToHash("Attack");
        private static readonly int AttackStateHash = Animator.StringToHash("Attack");
        private static readonly int AttackSpeedHash = Animator.StringToHash("AttackSpeed");

        [Header("Angriff")]
        [SerializeField] private LayerMask _enemyLayer;
        [SerializeField] private float _maxAttackDuration = 3f;     // Sicherheitsnetz, falls AttackEnd verpasst wird

        [Header("Projektil")]
        [SerializeField] private GameObject _projectilePrefab;
        [SerializeField] private Vector3 _spawnOffset = new Vector3(0f, 0.5f, 0.5f);   // etwas höher und vor dem Player
        [SerializeField] private PoolManager _poolManager;   // leer lassen, wird dann in der Szene gesucht

        [Header("Sound")]
        [SerializeField] private AudioEventChannel _audioChannel;
        [SerializeField] private SoundData _shootSound;

        private readonly Collider[] _hits = new Collider[64];
        private Animator _animator;
        private PlayerDataSO _data;
        private Collider _target;
        private bool _isAttacking;
        private bool _enteredAttackState;
        private float _timer;
        private int _attackCount;

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
            if (_poolManager == null) _poolManager = FindAnyObjectByType<PoolManager>();
        }

        private void Start()
        {
            _data = GetComponent<PlayerSetup>().Data;
            _animator.SetFloat(AttackSpeedHash, _data.attackSpeed);
        }

        private void Update()
        {
            _timer += Time.deltaTime;

            if (_isAttacking)
            {
                CheckAttackStateLeft();
                if (_isAttacking && _timer >= _maxAttackDuration) OnAttackFinished();
                return;
            }

            if (_timer < _data.attackInterval) return;

            Collider nearest = FindNearestEnemy();
            if (nearest == null) return;   // Bereitschaft, Timer läuft weiter

            _target = nearest;
            _isAttacking = true;
            _timer = 0f;
            _attackCount = _data.projectileCount;

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
            GameObject projectile = _poolManager.Spawn(_projectilePrefab, spawnPosition, Quaternion.identity);
            projectile.GetComponent<PlayerProjectile>().Launch(_target, _data.projectileDamage, _data.projectileBounces, _enemyLayer, _poolManager);
        }

        // Wird über AE_OnAttackEndFrame aufgerufen
        public void OnAttackFinished()
        {
            if(_attackCount > 1)
            {
                _attackCount--;
                _animator.SetTrigger(AttackHash);
                return;
            }
            _isAttacking = false;
            _enteredAttackState = false;
            _target = null;
            _timer = 0f;
        }

        // Ersatz für AE_OnAttackEndFrame: Angriff endet, sobald der Animator den Attack-State verlassen hat
        private void CheckAttackStateLeft()
        {
            bool inAttack = _animator.GetCurrentAnimatorStateInfo(0).shortNameHash == AttackStateHash
                || (_animator.IsInTransition(0) && _animator.GetNextAnimatorStateInfo(0).shortNameHash == AttackStateHash);

            if (inAttack) _enteredAttackState = true;
            else if (_enteredAttackState) OnAttackFinished();
        }

        private Collider FindNearestEnemy()
        {
            int hitCount = Physics.OverlapSphereNonAlloc(transform.position, _data.attackRange, _hits, _enemyLayer);
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
            // Im Editor läuft Awake nicht, deshalb Reichweite direkt aus dem PlayerSetup lesen
            PlayerSetup setup = GetComponent<PlayerSetup>();
            if (setup == null || setup.Data == null) return;

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, setup.Data.attackRange);
        }
    }
}