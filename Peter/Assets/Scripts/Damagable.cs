using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damagable : MonoBehaviour
{
    private float health = 100f;
    public float Health
    {
        get => health;
        set
        {
            health = value;
            UpdateHealth();
            CheckIfDead();
        }
    }

    [SerializeField]
    protected float maxHealth = 10f;

    public void Start()
    {
        Health = maxHealth;
    }

    private void CheckIfDead()
    {
        if (Health <= 0)
        {
            OnDeath();
        }
    }

    protected virtual void UpdateHealth()
    {

    }

    public virtual void DoDamage(float damage)
    {
        Health -= damage;
    }

    public void Heal()
    {

    }

    protected virtual void OnDeath()
    {
        Destroy(gameObject);
    }
}