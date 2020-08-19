using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BetterDamagable : Damagable
{
    [Header("Custom Damagable")]
    public GameObject thingToSpawnAtDeath;

    [Header("Healthbar")]
    [SerializeField] protected Slider slider;
    [SerializeField] protected Gradient gradient;
    [SerializeField] protected Image fill;

    [Header("Damage Numbers")]
    [SerializeField] protected DamageNumber damageNumber;
    [SerializeField] protected float radiusWhereTheNumbersSpawn = 1f;
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
        UIManager.Instance.ActivateHitMarker();
        SpawnDamageNumber(damage);

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

    private void SpawnDamageNumber(float amount)
    {
        if (damageNumber != null)
        {
            Instantiate(damageNumber, transform.position + Vector3.one * Random.Range(-radiusWhereTheNumbersSpawn, radiusWhereTheNumbersSpawn), Quaternion.identity, GameManager.Instance.ParticleHolder).Setup(amount);
        }
    }
}
