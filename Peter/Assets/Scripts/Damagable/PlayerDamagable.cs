using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamagable : Damagable
{
    protected override void UpdateHealth()
    {
        UIManager.Instance.UpdateHealthBar(Health / MaxHealth);
        base.UpdateHealth();
    }
}
