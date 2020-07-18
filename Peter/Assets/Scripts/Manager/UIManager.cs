using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : ManagerModule<UIManager>
{
    [Header("Health Bar")]
    [SerializeField] protected Slider healthBar;

    [Header("Switch Dimension Timer")]
    [SerializeField] protected Slider switchDimensionBar;

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

    /// <summary>
    /// Updates the switch dimension bar bar
    /// </summary>
    /// <param name="value">Value, between 1 and 0</param>
    public void UpdateSwitchDimensionSlider(float value)
    {
        if (switchDimensionBar != null)
        {
            switchDimensionBar.value = value;
        }
    }
}
