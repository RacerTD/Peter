using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Ability : MonoBehaviour
{
    [Header("Basic Ability Features")]
    public float AbilityDuration = 0f;
    private float abilityDurationTime = 0f;
    public bool AbilityActive = false;
    public float CoolDownDuration = 0f;
    private float coolDownDurationTime = 0f;
    public bool CoolDownActive = false;

    protected InputAction.CallbackContext currentInputAction;

    #region Ability

    /// <summary>
    /// What happens at the start of the ability
    /// </summary>
    public virtual void AbilityStart(InputAction.CallbackContext context)
    {
        currentInputAction = context;

        if (!CoolDownActive && !AbilityActive && context.started)
        {
            AbilityActive = true;
            AbilityCoolDownStart();
        }
    }

    /// <summary>
    /// What happens when the ability is active
    /// </summary>
    public virtual void AbilityUpdate()
    {
        abilityDurationTime += Time.deltaTime;

        if (abilityDurationTime >= AbilityDuration)
        {
            AbilityEnd();
        }
    }

    /// <summary>
    /// What happens at the end of the ability
    /// </summary>
    public virtual void AbilityEnd()
    {
        AbilityActive = false;
        abilityDurationTime = 0f;
    }

    #endregion

    #region CoolDown

    /// <summary>
    /// What happens at the start of the cooldown
    /// </summary>
    public virtual void AbilityCoolDownStart()
    {
        if (CoolDownDuration > 0f)
        {
            CoolDownActive = true;
        }
    }

    /// <summary>
    /// What happens during the cooldown
    /// </summary>
    public virtual void AbilityCoolDownUpdate()
    {
        coolDownDurationTime += Time.deltaTime;

        if (coolDownDurationTime >= CoolDownDuration)
        {
            AbilityCoolDownEnd();
        }
    }

    /// <summary>
    /// What happens at the end of the ability cooldown
    /// </summary>
    public virtual void AbilityCoolDownEnd()
    {
        CoolDownActive = false;
        coolDownDurationTime = 0f;
    }

    #endregion
}
