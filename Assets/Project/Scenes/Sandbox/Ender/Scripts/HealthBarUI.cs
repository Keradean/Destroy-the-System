using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Image currentHealthBar;
    [SerializeField] private Image lostHealthBar;

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