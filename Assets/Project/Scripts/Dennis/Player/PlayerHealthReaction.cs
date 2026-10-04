using Project.Scripts.Dennis.Audio;
using Project.Scripts.Dennis.Game;
using UnityEngine;

namespace Project.Scripts.Dennis.Player
{
    // Reagiert auf Schaden und Tod: Animationen starten, Steuerung sperren
    [RequireComponent(typeof(HealthComponent))]
    public class PlayerHealthReaction : MonoBehaviour
    {
        private static readonly int DamageHash = Animator.StringToHash("Damage");
        private static readonly int DieHash = Animator.StringToHash("Die");
        private static readonly int DieStateHash = Animator.StringToHash("Die");

        [SerializeField] private GameManager _gameManager;

        [Header("Sound")]
        [SerializeField] private AudioEventChannel _audioChannel;
        [SerializeField] private SoundData _damageSound;

        private HealthComponent _health;
        private Animator _animator;
        private PlayerMoveComponent _move;
        private PlayerAttack _attack;
        private float _lastHealth;

        private void Awake()
        {
            _health = GetComponent<HealthComponent>();
            _animator = GetComponentInChildren<Animator>();
            _move = GetComponent<PlayerMoveComponent>();
            _attack = GetComponent<PlayerAttack>();
            if (_gameManager == null) _gameManager = FindAnyObjectByType<GameManager>();
        }

        private void OnEnable()
        {
            _health.OnHealthChanged += HandleHealthChanged;
            _health.OnDeath += HandleDeath;
        }

        private void OnDisable()
        {
            _health.OnHealthChanged -= HandleHealthChanged;
            _health.OnDeath -= HandleDeath;
        }

        private void Start()
        {
            _lastHealth = _health.CurrentHealth;
        }

        private void HandleHealthChanged(float current, float max)
        {
            bool tookDamage = current < _lastHealth;
            _lastHealth = current;

            if (tookDamage && !_health.IsDead)
            {
                _animator.SetTrigger(DamageHash);
                if (_audioChannel != null) _audioChannel.RaiseSFX(_damageSound, transform.position);
            }
        }

        private void HandleDeath()
        {
            // Steuerung und Angriff sofort sperren
            _move.enabled = false;
            _attack.enabled = false;

            // Ohne Die-State im Animator kommt AE_OnDeathEndFrame nie, dann direkt weiter
            if (!_animator.HasState(0, DieStateHash))
            {
                OnDeathAnimationFinished();
                return;
            }
            _animator.SetTrigger(DieHash);
        }

        // Wird über AE_OnDeathEndFrame aufgerufen
        public void OnDeathAnimationFinished()
        {
            _gameManager.Lose();
        }
    }
}