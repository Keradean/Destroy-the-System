using UnityEngine;

namespace Project.Scenes.Sandbox.Ender.Scripts
{
    public class HealthBarUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private RectTransform healthFill;
        [SerializeField] private RectTransform healthEmptyBG;
        [SerializeField] private RectTransform healthWaveBlue;

        [Header("Health - Test Values")]
        [SerializeField] private float maxHealth = 100f;
        [SerializeField] private float currentHealth = 100f;

        [Header("Blue Wave Pulse")]
        [SerializeField] private float minScaleY = 0.85f;
        [SerializeField] private float maxScaleY = 1.2f;
        [SerializeField] private float minPulseSpeed = 0.5f;
        [SerializeField] private float maxPulseSpeed = 1.8f;

        private float targetScaleY;
        private float pulseSpeed;

        private void Start()
        {
            SetHealth(currentHealth, maxHealth);
            PickNewPulseTarget();
        }

        private void Update()
        {
            UpdateWavePulse();
        }

        // Wird vom Gameplay-Code aufgerufen, wenn sich die Lebenspunkte ändern.
        // Beispiel: healthBarUI.SetHealth(75f, 100f);
        // current = aktuelle Lebenspunkte
        // max = maximale Lebenspunkte
    
        public void SetHealth(float current, float max)
        {
            maxHealth = Mathf.Max(1f, max);
            currentHealth = Mathf.Clamp(current, 0f, maxHealth);

            UpdateHealthBar();
        }

        private void UpdateHealthBar()
        {
            float healthPercent = currentHealth / maxHealth;

            Vector2 fillMax = healthFill.anchorMax;
            fillMax.x = healthPercent;
            healthFill.anchorMax = fillMax;

            Vector2 emptyMin = healthEmptyBG.anchorMin;
            emptyMin.x = healthPercent;
            healthEmptyBG.anchorMin = emptyMin;
        }

        private void UpdateWavePulse()
        {
            if (!healthWaveBlue)
                return;

            Vector3 scale = healthWaveBlue.localScale;
            scale.y = Mathf.MoveTowards(scale.y, targetScaleY, pulseSpeed * Time.deltaTime);
            healthWaveBlue.localScale = scale;

            if (Mathf.Abs(scale.y - targetScaleY) < 0.01f)
            {
                PickNewPulseTarget();
            }
        }

        private void PickNewPulseTarget()
        {
            targetScaleY = Random.Range(minScaleY, maxScaleY);
            pulseSpeed = Random.Range(minPulseSpeed, maxPulseSpeed);
        }
    }
}