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
    private bool abilityHasDuration = false;
    [HideInInspector] public bool AbilityActive = false;
    public float CoolDownDuration = 0f;
    private float coolDownDurationTime = 0f;
    [HideInInspector] public bool CoolDownActive = false;
    [HideInInspector] public InputAction.CallbackContext currentInputAction = new InputAction.CallbackContext();
    [HideInInspector] public AbilityController controller;

    protected virtual void Start()
    {
        abilityHasDuration = abilityDurationTime <= 0f;
    }

    protected virtual void Update()
    {

    }

    public virtual void GetInput(InputAction.CallbackContext context)
    {
        currentInputAction = context;

        if (context.started && !CoolDownActive)
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

    public virtual void CheckForCoolDown()
    {

    }

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
        coolDownDurationTime -= Time.deltaTime;

        if (coolDownDurationTime <= 0)
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
        coolDownDurationTime = CoolDownDuration;
    }

    #endregion
}
