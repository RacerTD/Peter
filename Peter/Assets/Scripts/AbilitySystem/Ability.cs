using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Ability : MonoBehaviour
{
    [Header("Basic Ability Features")]
    public float AbilityDuration = 0f;
    protected float abilityDurationTime = 0f;
    protected bool abilityHasDuration = false;
    [HideInInspector] public bool AbilityActive = false;
    public float CoolDownDuration = 0f;
    [HideInInspector] public float CoolDownDurationTime = 0f;
    [HideInInspector] public AbilityController controller;

    #region Input

    [HideInInspector] public bool InputStarted = false;
    [HideInInspector] public bool InputPerformed = false;
    [HideInInspector] public bool InputCanceled = false;

    #endregion

    protected virtual void Start()
    {
        abilityHasDuration = abilityDurationTime <= 0f;
    }

    protected virtual void Update()
    {

    }

    /// <summary>
    /// Saves the inputAction required for this ability
    /// </summary>
    public virtual void GetInput(InputAction.CallbackContext context)
    {
        InputStarted = context.started;
        InputPerformed = context.performed;
        InputCanceled = context.canceled;

        if (context.started && CoolDownDurationTime <= 0)
        {
            AbilityStart();
        }

        //Debug.Log($"{context.started} {context.performed} {context.canceled} current {currentInputAction.started} {currentInputAction.performed} {currentInputAction.canceled}");
    }

    #region Ability

    /// <summary>
    /// What happens at the start of the ability
    /// </summary>
    public virtual void AbilityStart()
    {
        AbilityActive = true;
        AbilityCoolDownStart();
    }

    /// <summary>
    /// What happens when the ability is active
    /// </summary>
    public virtual void AbilityUpdate()
    {
        if (abilityHasDuration)
        {
            abilityDurationTime += Time.deltaTime;

            if (abilityDurationTime >= AbilityDuration && AbilityDuration > 0f)
            {
                AbilityEnd();
            }
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
        CoolDownDurationTime = CoolDownDuration;
    }

    /// <summary>
    /// What happens during the cooldown
    /// </summary>
    public virtual void AbilityCoolDownUpdate()
    {
        CoolDownDurationTime -= Time.deltaTime;

        if (CoolDownDurationTime <= 0)
        {
            AbilityCoolDownEnd();
        }
    }

    /// <summary>
    /// What happens at the end of the ability cooldown
    /// </summary>
    public virtual void AbilityCoolDownEnd()
    {

    }

    #endregion
}
