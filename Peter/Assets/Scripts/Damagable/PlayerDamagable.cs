using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamagable : Damagable
{
    private float timeSinceLastHit = 0f;
    [SerializeField] private float timeTillHeal = 10f;
    [SerializeField] private float healPerSecond = 1f;

    protected override void Update()
    {
        timeSinceLastHit += Time.deltaTime;

        if (timeSinceLastHit >= timeTillHeal)
        {
            Heal(healPerSecond * Time.deltaTime);
        }
        base.Update();
    }

    public override void Heal(float amount)
    {
        Health += amount;
        base.Heal(amount);
    }

    protected override void UpdateHealth()
    {
        UIManager.Instance.UpdateHealthBar(Health / MaxHealth);
        base.UpdateHealth();
    }

    public override void DoDamage(float damage)
    {
        timeSinceLastHit = 0f;
        base.DoDamage(damage);
    }

    protected override void OnDeath()
    {
        if (GameManager.Instance.CurrentGameState != GameState.Dead)
        {
            GameManager.Instance.CurrentGameState = GameState.Dead;
        }
    }
}
