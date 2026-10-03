using UnityEngine;

namespace Project.Scripts.Dennis.Player
{
    // Fliegt auf einen Gegner zu und trifft ihn
    public class PlayerProjectile : MonoBehaviour
    {
        [SerializeField] private float _speed = 12f;
        [SerializeField] private float _hitDistance = 0.3f;
        [SerializeField] private float _maxLifetime = 3f;

        private Collider _target;
        private float _damage;
        private PoolManager _poolManager;
        private float _lifetime;

        // Schaden kommt vom Player, damit Upgrades ihn später ändern können.
        // Wird bei jedem Schuss neu aufgerufen, weil das Projektil aus dem Pool wiederverwendet wird.
        public void Launch(Collider target, float damage, PoolManager poolManager)
        {
            _target = target;
            _damage = damage;
            _poolManager = poolManager;
            _lifetime = 0f;
        }

        private void Update()
        {
            // Sicherheitsnetz, falls es nie ankommt
            _lifetime += Time.deltaTime;
            if (_lifetime >= _maxLifetime)
            {
                ReturnToPool();
                return;
            }

            // Ziel tot oder deaktiviert: Projektil verschwindet
            if (_target == null || !_target.enabled || !_target.gameObject.activeInHierarchy)
            {
                ReturnToPool();
                return;
            }

            Vector3 targetPosition = _target.bounds.center;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, _speed * Time.deltaTime);
            transform.LookAt(targetPosition);

            if (Vector3.Distance(transform.position, targetPosition) > _hitDistance) return;

            Hit();
        }

        private void Hit()
        {
            HealthComponent health = _target.GetComponentInParent<HealthComponent>();
            if (health != null)
            {
                health.TakeDamage(_damage);
            }

            // TODO: später Impact-Animation abspielen, erst danach zurück in den Pool
            ReturnToPool();
        }

        private void ReturnToPool()
        {
            _target = null;
            _poolManager.Despawn(gameObject);
        }
    }
}