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

        // Schaden kommt vom Player, damit Upgrades ihn später ändern können
        public void Launch(Collider target, float damage)
        {
            _target = target;
            _damage = damage;
            Destroy(gameObject, _maxLifetime);   // Sicherheitsnetz, falls es nie ankommt
        }

        private void Update()
        {
            // Ziel tot oder deaktiviert: Projektil verschwindet
            if (_target == null || !_target.enabled || !_target.gameObject.activeInHierarchy)
            {
                Destroy(gameObject);
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

            // TODO: später Impact-Animation abspielen und Projektil in den Pool statt Destroy
            Destroy(gameObject);
        }
    }
}