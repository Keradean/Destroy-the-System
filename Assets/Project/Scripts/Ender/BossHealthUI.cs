using UnityEngine;

public class BossHealthUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject bossHealthRoot;
    [SerializeField] private RectTransform bossHealthFill;

    [Header("Fill Positions")]
    [SerializeField] private float fullPosX = -102.8f;
    [SerializeField] private float emptyPosX = -722.7f;

    private HealthComponent bossHealth;

    private void Awake()
    {
        if (bossHealthRoot)
        {
            bossHealthRoot.SetActive(false);
        }
    }

    private void OnDisable()
    {
        if (bossHealth)
        {
            bossHealth.OnHealthChanged -= SetHealth;
            bossHealth.OnDeath -= HideBossHealth;
        }
    }

    public void SetBoss(HealthComponent health)
    {
        if (bossHealth)
        {
            bossHealth.OnHealthChanged -= SetHealth;
            bossHealth.OnDeath -= HideBossHealth;
        }

        bossHealth = health;

        if (!bossHealth)
            return;

        if (bossHealthRoot)
        {
            bossHealthRoot.SetActive(true);
        }

        bossHealth.OnHealthChanged += SetHealth;
        bossHealth.OnDeath += HideBossHealth;

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

    private void HideBossHealth()
    {
        if (bossHealthRoot)
        {
            bossHealthRoot.SetActive(false);
        }
    }
}