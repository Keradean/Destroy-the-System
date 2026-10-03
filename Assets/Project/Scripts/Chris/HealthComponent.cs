using System;
using UnityEngine;

[AddComponentMenu("Entities/Components/HealthComponent")]
public class HealthComponent : MonoBehaviour
{
    public float CurrentHealth { get; private set; }
    public float MaxHealth { get; private set; }
    public bool IsDead => CurrentHealth <= 0;

    public event Action OnDeath;
    public event Action<float, float> OnHealthChanged;

    public void Initialize(float maxHealth)
    {
        MaxHealth = maxHealth;
        CurrentHealth = maxHealth;
        OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
    }

    public void TakeDamage(float amount)
    {
        if (IsDead) return;
        CurrentHealth = Mathf.Max(0f, CurrentHealth - amount);
        OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);

        if (IsDead) OnDeath?.Invoke();
    }
    public void Heal(float amount)
    {
        if (IsDead) return;

        CurrentHealth = Mathf.Min(MaxHealth, CurrentHealth + amount);
        OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
    }
}
