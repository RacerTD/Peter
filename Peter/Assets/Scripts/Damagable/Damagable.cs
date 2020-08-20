using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Damagable : MonoBehaviour
{
    #region MaxHealth
    [Header("Standard Damagable")]
    [SerializeField] private float maxHealth = 10f;
    public float MaxHealth
    {
        get => maxHealth;
        set
        {
            maxHealth = value;
            UpdateMaxHealth();
        }
    }
    #endregion

    #region Health
    private float health = 100f;
    public float Health
    {
        get => health;
        set
        {
            if (value > maxHealth)
            {
                health = maxHealth;
            }
            else
            {
                health = value;
            }

            UpdateHealth();
            CheckIfDead();
        }
    }
    #endregion

    [SerializeField] protected AudioClip onHitSound;
    [SerializeField] protected AudioClip onDeathSound;

    protected virtual void Start()
    {
        Health = maxHealth;
    }

    protected virtual void Update()
    {

    }

    /// <summary>
    /// Checks if the thing should die and starts OnDeath()
    /// </summary>
    protected virtual void CheckIfDead()
    {
        if (Health <= 0)
        {
            OnDeath();
        }
    }

    /// <summary>
    /// Happens every time the Health changes
    /// </summary>
    protected virtual void UpdateHealth()
    {

    }

    /// <summary>
    /// Happens every time the MaxHealth changes
    /// </summary>
    protected virtual void UpdateMaxHealth()
    {

    }

    public virtual void DoDamage(float damage)
    {
        Health -= damage;
        if (onHitSound != null)
        {
            AudioManager.Instance.PlayNewSound(AudioType.Sfx, onHitSound, gameObject);
        }
    }

    /// <summary>
    /// What happens when the thing gets healed
    /// </summary>
    public virtual void Heal(float amount)
    {

    }

    /// <summary>
    /// What happsens when the thing dies
    /// </summary>
    protected virtual void OnDeath()
    {
        if (onDeathSound != null)
        {
            AudioManager.Instance.PlayNewSound(AudioType.Sfx, onDeathSound, transform.position);
        }
        Destroy(gameObject);
    }
}