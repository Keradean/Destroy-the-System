using UnityEngine;

namespace Project.Scripts.Dennis.Player
{
    // Startwerte des Players. Wird zur Laufzeit nur gelesen, nie verändert.
    [CreateAssetMenu(fileName = "PlayerData", menuName = "Destroy the System/Player Data")]
    public class PlayerDataSO : ScriptableObject
    {
        [Header("Leben")]
        public float maxHealth = 100f;

        [Header("Bewegung")]
        public float moveSpeed = 6.5f;

        [Header("Angriff")]
        public float attackInterval = 0.5f;
        public float attackRange = 5f;
        public float projectileDamage = 10f;
    }
}