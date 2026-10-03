using UnityEngine;
using UnityEngine.UI;

namespace Project.Scenes.Sandbox.Ender.Scripts
{
    public class HealthBarUI : MonoBehaviour
    {
        [SerializeField] private Image currentHealthBar;
        [SerializeField] private Image lostHealthBar;

        // Wird vom Gameplay-Code aufgerufen, wenn sich die Lebenspunkte ändern.
        // Beispiel: healthBarUI.SetHealth(75f, 100f);
        // currentHealth = aktuelle Lebenspunkte
        // maxHealth = maximale Lebenspunkte
        public void SetHealth(float currentHealth, float maxHealth)
        {
            if (maxHealth <= 0f)
            {
                maxHealth = 1f;
            }

            float healthPercent = Mathf.Clamp01(currentHealth / maxHealth);

            if (currentHealthBar)
            {
                currentHealthBar.fillAmount = healthPercent;
            }

            if (lostHealthBar)
            {
                lostHealthBar.fillAmount = 1f - healthPercent;
            }
        }
    }
}