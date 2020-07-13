using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;
using UnityEngine.UI;
using UnityEngine.UIElements;

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
    public float maxHealth = 10f;

    public void Start()
    {
        Health = maxHealth;
    }

    protected virtual void CheckIfDead()
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

