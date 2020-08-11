﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;


public class UIManager : ManagerModule<UIManager>
{
    [Header("Health Bar")]
    [SerializeField] protected Slider healthBar;

    [Header("Switch Dimension Timer")]
    [SerializeField] protected Slider switchDimensionBar;

    #region HitMarker
    [Header("Hit Marker")]
    [SerializeField] protected List<Image> hitMarkers = new List<Image>();
    [SerializeField] protected Transform hitMarkerParent;
    [SerializeField] private Color hitMarkerStartColor = new Color(0, 0, 0, 0);
    [SerializeField] private Color hitMarkerActiveColor = Color.white;
    [SerializeField] private Color hitMarkerEndColor = new Color(0, 0, 0, 0);
    private float hitMarkerStartSize = 1f;
    [SerializeField] private float hitMarkerEndSize = 2f;
    [SerializeField] private float hitMarkerDamageSizeMultiplicator = 1f;
    [SerializeField] [Tooltip("Percent of the hit Marker active time the marker is in the stage of getting bigger and mor non transparent")] private float hitMarkerGetBiggerTimePercent = 0.7f;
    private float hitMarkerDamage = 0f;
    private bool hitMarkerIsActive = false;
    [SerializeField] private float hitMarkerTotalTime = 0f;
    private float hitMarkerTotalTimer = 0f;
    #endregion
    private void Start()
    {
        if (hitMarkers.Count > 0)
        {
            hitMarkerStartSize = hitMarkerParent.localScale.x;
        }
    }

    private void Update()
    {
        HandleHitMarker();
    }

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

    /// <summary>
    /// Starts the hit marker
    /// </summary>
    public void ActivateHitMarker(float damage)
    {
        if (hitMarkers.Count > 0)
        {
            hitMarkerIsActive = true;
            hitMarkerTotalTimer = 0f;
            /*
            foreach (Image rend in hitMarkers)
            {
                rend.rectTransform.localScale = Vector3.one * hitMarkerStartSize;
            }
            */
        }
    }

    /// <summary>
    /// Handles all the things needed for a smooth hit marker
    /// </summary>
    private void HandleHitMarker()
    {
        if (hitMarkerIsActive)
        {
            if (hitMarkerTotalTimer <= hitMarkerTotalTime * hitMarkerGetBiggerTimePercent)
            {
                foreach (Image rend in hitMarkers)
                {
                    rend.color = Color.Lerp(hitMarkerStartColor, hitMarkerActiveColor, hitMarkerTotalTimer / (hitMarkerTotalTime * hitMarkerGetBiggerTimePercent));
                }

                hitMarkerParent.localScale = Vector3.Lerp(hitMarkerParent.localScale, Vector3.one * hitMarkerEndSize, hitMarkerTotalTimer / (hitMarkerTotalTime * hitMarkerGetBiggerTimePercent));
            }
            else
            {
                foreach (Image rend in hitMarkers)
                {
                    rend.color = Color.Lerp(hitMarkerActiveColor, hitMarkerEndColor, (hitMarkerTotalTimer - hitMarkerTotalTime * hitMarkerGetBiggerTimePercent) / (hitMarkerTotalTime * hitMarkerGetBiggerTimePercent));
                }

                hitMarkerParent.localScale = Vector3.Lerp(hitMarkerParent.localScale, Vector3.one * hitMarkerStartSize, (hitMarkerTotalTimer - hitMarkerTotalTime * hitMarkerGetBiggerTimePercent) / (hitMarkerTotalTime * hitMarkerGetBiggerTimePercent));
            }

            if (hitMarkerTotalTimer >= hitMarkerTotalTime)
            {
                hitMarkerIsActive = false;
                hitMarkerParent.localScale = Vector3.one * hitMarkerStartSize;

                foreach (Image rend in hitMarkers)
                {
                    rend.color = hitMarkerEndColor;
                }
            }

            hitMarkerTotalTimer += Time.deltaTime;
        }
    }
}
