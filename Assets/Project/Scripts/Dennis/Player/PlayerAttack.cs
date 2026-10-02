using UnityEngine;

namespace Project.Scripts.Dennis.Player
{
    public class PlayerAttack : MonoBehaviour 
    {
        [SerializeField] private float _attackInterval = 1f;
        [SerializeField] private float range = 5f;
        [SerializeField] private LayerMask _enemyLayer;
        
        //Intern:
        // Collider[] _hits, gleich hier mit new Collider[32] anlegen
        Collider[] hits = new Collider[32];
        float _timer = 0f;

        void Update()
        {
            TryAttack();
        }

        private void TryAttack()
        {
            //timer hochzählen
            _timer += Time.deltaTime;
            //Noch nicht so weit? raus.
            if (_timer < _attackInterval) return;
            //Nächsten Gegner suchen, in einer Variable merken
            Transform nearestEnemy = FindNearestEnemy();
            //Keiner da? raus.(Bereitschaft, Timer NICHT zurücksetzen)
            if (nearestEnemy == null) return;
            //Timer auf null
            _timer = 0f;
            // Debug log mit dem namen des Ziels
            Debug.Log($"Attacking {nearestEnemy.name}");
        }
        
        private Transform FindNearestEnemy()
        {
            int hitCount = Physics.OverlapSphereNonAlloc(transform.position, range, hits, _enemyLayer);
            Transform nearestEnemy = null;
            float nearestDistance = Mathf.Infinity;

            for (int i = 0; i < hitCount; i++)
            {
                float distance = Vector3.Distance(transform.position, hits[i].transform.position);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestEnemy = hits[i].transform;
                }
            }
            return nearestEnemy;
        }
    }
}