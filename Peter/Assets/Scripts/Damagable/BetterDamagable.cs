using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BetterDamagable : Damagable
{
    [Header("Custom Damagable")]
    public GameObject thingToSpawnAtDeath;
    [SerializeField] protected Slider slider;
    [SerializeField] protected Gradient gradient;
    [SerializeField] protected Image fill;
    private Transform cam;
    private EnemyController enemyController;

    protected override void Start()
    {
        enemyController = GetComponent<EnemyController>();
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

    public override void DoDamage(float damage)
    {
        enemyController.GotShotAt();
        base.DoDamage(damage);
    }

    protected override void OnDeath()
    {
        if (thingToSpawnAtDeath != null)
        {
            Instantiate(thingToSpawnAtDeath, transform.position, transform.rotation);
        }
        base.OnDeath();
    }
}
