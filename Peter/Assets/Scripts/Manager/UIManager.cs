using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : ManagerModule<UIManager>
{
    [Header("Health Bar")]
    [SerializeField] protected Slider healthBar;

    /// <summary>
    /// Updates the health bar
    /// </summary>
    /// <param name="value">Value, between 1 and 0</param>
    public void UpdateHealthBar(float value)
    {
        if (healthBar != null)
        {
            healthBar.value = value;
        }
    }
}
