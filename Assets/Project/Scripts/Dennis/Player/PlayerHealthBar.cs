using Project.Scenes.Sandbox.Ender.Scripts;
using UnityEngine;

namespace Project.Scripts.Dennis.Player
{
    // Verbindet das Leben des Players mit Enders Lebensbalken
    [RequireComponent(typeof(HealthComponent))]
    public class PlayerHealthBar : MonoBehaviour
    {
        [SerializeField] private HealthBarUI _healthBar;   // leer lassen, wird dann in der Szene gesucht

        private HealthComponent _health;

        private void Awake()
        {
            _health = GetComponent<HealthComponent>();
            if (_healthBar == null) _healthBar = FindAnyObjectByType<HealthBarUI>();
        }

        private void OnEnable()
        {
            _health.OnHealthChanged += UpdateBar;
        }

        private void OnDisable()
        {
            _health.OnHealthChanged -= UpdateBar;
        }

        private void Start()
        {
            // PlayerSetup setzt das Leben schon in Awake, das Event kam also vor dem Anmelden
            UpdateBar(_health.CurrentHealth, _health.MaxHealth);
        }

        private void UpdateBar(float current, float max)
        {
            if (_healthBar == null) return;
            _healthBar.SetHealth(current, max);
        }
    }
}
