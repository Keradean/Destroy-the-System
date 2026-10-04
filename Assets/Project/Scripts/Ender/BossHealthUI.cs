using UnityEngine;

public class BossHealthUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private RectTransform bossHealthFill;

    [Header("Fill Positions")]
    [SerializeField] private float fullPosX = -90.4f;
    [SerializeField] private float emptyPosX = -712.4f;

    private HealthComponent bossHealth;

    private void OnDisable()
    {
        if (bossHealth)
        {
            bossHealth.OnHealthChanged -= SetHealth;
        }
    }

    public void SetBoss(HealthComponent health)
    {
        if (bossHealth)
        {
            bossHealth.OnHealthChanged -= SetHealth;
        }

        bossHealth = health;

        if (!bossHealth)
            return;

        bossHealth.OnHealthChanged += SetHealth;
        SetHealth(bossHealth.CurrentHealth, bossHealth.MaxHealth);
    }

    private void SetHealth(float currentHealth, float maxHealth)
    {
        if (maxHealth <= 0f)
        {
            maxHealth = 1f;
        }

        float healthPercent = Mathf.Clamp01(currentHealth / maxHealth);
        float newPosX = Mathf.Lerp(emptyPosX, fullPosX, healthPercent);
        Vector2 position = bossHealthFill.anchoredPosition;
        position.x = newPosX;
        bossHealthFill.anchoredPosition = position;
    }
}