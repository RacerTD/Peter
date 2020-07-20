using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerShoot))]
public class PlayerSwitchDimension : Ability
{
    [Header("Custom Ability Features")]
    [SerializeField] protected Vector3 DimensionOffset = Vector3.zero;
    [HideInInspector] public bool DimA = true;
    [SerializeField] private float damageDecreaseFactor = 1f;
    private PlayerShoot playerShoot;

    protected override void Start()
    {
        UIManager.Instance.UpdateSwitchDimensionSlider(CoolDownDurationTime / CoolDownDuration);
        playerShoot = GetComponent<PlayerShoot>();
        base.Start();
    }

    public override void AbilityStart()
    {
        if (DimA)
        {
            transform.position += DimensionOffset;
        }
        else
        {
            transform.position -= DimensionOffset;
        }

        DimA = !DimA;
        playerShoot.UpdateAmmoDisplay();

        base.AbilityStart();
    }

    public override void AbilityCoolDownUpdate()
    {
        base.AbilityCoolDownUpdate();
    }

    public override void AbilityCoolDownEnd()
    {
        base.AbilityCoolDownEnd();
    }

    public void DecreaseSwitchTimer(float damage)
    {
        CoolDownDurationTime -= damage * damageDecreaseFactor;
    }

    public override void UpdateCoolDownDurationTime()
    {
        UIManager.Instance.UpdateSwitchDimensionSlider(CoolDownDurationTime / CoolDownDuration);
        base.UpdateCoolDownDurationTime();
    }
}
