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
        }
    }

    private void UpdateHealth()
    {

    }

    public void DoDamage(float damage)
    {
        Health -= damage;
    }

    public void Heal()
    {

    }
}