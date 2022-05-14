using System;
using UnityEngine;
using UnityEngine.Video;

public class HealthSystem : MonoBehaviour
{
    public event EventHandler OnHealthAmountMaxChanged;
    public event EventHandler OnDamaged;
    public event EventHandler OnHealed;
    public event EventHandler OnDied;

    [SerializeField] private int healthAmountMax;
    private int healthAmount;

    private void Awake()
    {
        healthAmount = healthAmountMax;
    }

    public void Damage(int damageAmount)
    {
        healthAmount -= damageAmount;
        healthAmount = Mathf.Clamp(healthAmount, 0, healthAmountMax);
        
        OnDamaged?.Invoke(this, EventArgs.Empty);
        
        if (IsDead())
            OnDied?.Invoke(this, EventArgs.Empty);
    }

    private bool IsDead() => healthAmount == 0;

    public int GetHealthAmount() => healthAmount;
    
    public int GetHealthAmountMax() => healthAmountMax;

    public float GetHealthAmountNormalized() => (float) healthAmount / healthAmountMax;

    public bool IsFullHealth() => healthAmount == healthAmountMax;

    public void SetHealthAmountMax(int healthAmountMax, bool updateHealthAmount)
    {
        this.healthAmountMax = healthAmountMax;

        if (updateHealthAmount)
            healthAmount = healthAmountMax;
        
        OnHealthAmountMaxChanged?.Invoke(this, EventArgs.Empty);
    }

    public void Heal(int healAmount)
    {
        healthAmount += healAmount;
        healthAmount = Mathf.Clamp(healthAmount, 0, healthAmountMax);
        
        OnHealed?.Invoke(this, EventArgs.Empty);
    }

    public void HealFull()
    {
        healthAmount = healthAmountMax;
        
        OnHealed?.Invoke(this, EventArgs.Empty);
    }
}