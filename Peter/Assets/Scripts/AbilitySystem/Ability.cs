using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Ability : MonoBehaviour
{
    [Header("Basic Ability Features")]
    public float AbilityDuration = 0f;
    protected bool abilityHasDuration = false;
    public float CoolDownDuration = 0f;
    public float TimeSinceLastUse = 0f;
    public float TimeBlocked = 0f;

    [HideInInspector] public AbilityController controller;

    #region Input

    [HideInInspector] public bool InputStarted = false;
    [HideInInspector] public bool InputPerformed = false;
    [HideInInspector] public bool InputCanceled = false;

    #endregion

    public virtual void PermanentUpdate()
    {
        if (!(AbilityDuration <= 0f) && !(CoolDownDuration <= 0f))
        {
            TimeSinceLastUse += Time.deltaTime;
        }

        TimeBlocked -= Time.deltaTime;

        if (TimeSinceLastUse <= AbilityDuration && TimeSinceLastUse + Time.deltaTime > AbilityDuration)
        {
            AbilityEnd();
            AbilityCoolDownStart();
        }

        if (TimeSinceLastUse <= AbilityDuration + CoolDownDuration && TimeSinceLastUse + Time.deltaTime > AbilityDuration + CoolDownDuration)
        {
            AbilityEnd();
        }
    }

    /// <summary>
    /// Saves the inputAction required for this ability
    /// </summary>
    public virtual void GetInput(InputAction.CallbackContext context)
    {
        InputStarted = context.started;
        InputPerformed = context.performed;
        InputCanceled = context.canceled;

        if (context.started && TimeSinceLastUse >= AbilityDuration + CoolDownDuration && TimeBlocked <= 0f)
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
        TimeSinceLastUse = 0f;
    }

    /// <summary>
    /// What happens when the ability is active
    /// </summary>
    public virtual void AbilityUpdate()
    {

    }

    /// <summary>
    /// What happens at the end of the ability
    /// </summary>
    public virtual void AbilityEnd()
    {

    }

    #endregion

    #region CoolDown

    /// <summary>
    /// What happens at the start of the cooldown
    /// </summary>
    public virtual void AbilityCoolDownStart()
    {

    }

    /// <summary>
    /// What happens during the cooldown
    /// </summary>
    public virtual void AbilityCoolDownUpdate()
    {
        UpdateCoolDownDurationTime();

        if (TimeSinceLastUse > CoolDownDuration + AbilityDuration)
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

    /// <summary>
    /// What happens when the input device changes
    /// </summary>
    public virtual void OnDeviceChanged(PlayerInput input)
    {

    }

    /// <summary>
    /// Happens every time the cooldown changes
    /// </summary>
    public virtual void UpdateCoolDownDurationTime()
    {

    }

    /// <summary>
    /// Returns if the Ablity is currently active
    /// </summary>
    public bool AbilityOnCoolDown()
    {
        if (TimeSinceLastUse > AbilityDuration && TimeSinceLastUse < AbilityDuration + CoolDownDuration)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Returns if the ability is currently on cooldown
    /// </summary>
    public bool AbilityActive()
    {
        if (TimeSinceLastUse <= AbilityDuration)
        {
            return true;
        }

        return false;
    }
}
