using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;
using UnityEngine.Events;

public class BetterDamagable : Damagable
{
    [Header("Custom Damagable")]
    public GameObject thingToSpawnAtDeath;
    public VisualEffect deathEffect;
    public UnityEvent m_OnHitEvent = new UnityEvent();

    [Header("Healthbar")]
    [SerializeField] protected Slider slider;
    [SerializeField] protected Gradient gradient;
    [SerializeField] protected Image fill;

    [Header("Damage Numbers")]
    [SerializeField] protected DamageNumber damageNumber;
    [SerializeField] protected float radiusWhereTheNumbersSpawn = 1f;
    [SerializeField] protected Vector3 damageNumberSpawnOffset = Vector3.up;
    private Transform cam;
    private EnemyController enemyController;
    [SerializeField] protected VisualEffect onHitEffect;

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
        if (onHitEffect != null)
        {
            Instantiate(onHitEffect, transform.position, transform.rotation, GameManager.Instance.ParticleHolder);
        }

        m_OnHitEvent.Invoke();

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
        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, transform.rotation);
        }
        base.OnDeath();
    }

    private void SpawnDamageNumber(float amount)
    {
        if (damageNumber != null)
        {
            Instantiate(damageNumber, transform.position + damageNumberSpawnOffset + Vector3.one * Random.Range(-radiusWhereTheNumbersSpawn, radiusWhereTheNumbersSpawn), Quaternion.identity, GameManager.Instance.ParticleHolder).Setup(amount);
        }
    }
}