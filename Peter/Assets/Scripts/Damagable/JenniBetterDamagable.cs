using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JenniBetterDamagable : Damagable
{
    [Header("Custom Damagable")]
    public GameObject thingToSpawnAtDeath;
    [SerializeField] protected Slider slider;
    [SerializeField] protected Gradient gradient;
    [SerializeField] protected Image fill;
    [SerializeField] protected DamageNumbers dmgNumbers;
    private Transform cam;

    protected override void Start()
    {
        UpdateMaxHealth();
        cam = Camera.main.transform;
        base.Start();
    }

    protected override void Update()
    {
        if (slider != null)
        {
            slider.transform.LookAt(cam);
        }

        base.Update();
    }

    protected override void UpdateHealth()
    {
        if (slider != null)
        {
            
            slider.value = Health / MaxHealth;
            fill.color = gradient.Evaluate(slider.normalizedValue);
        }

        base.UpdateHealth();
    }

    protected override void OnDeath()
    {
        if (thingToSpawnAtDeath != null)
        {
            Instantiate(thingToSpawnAtDeath, transform.position, transform.rotation);
        }
        base.OnDeath();
    }

    public override void DoDamage(float damage)
    {
        dmgNumbers.CreateFloatingText(damage.ToString(), transform);
        base.DoDamage(damage);
    }
}
