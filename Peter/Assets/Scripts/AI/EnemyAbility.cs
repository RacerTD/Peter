using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyController))]
public abstract class EnemyAbility : MonoBehaviour
{
    [HideInInspector] public EnemyController Controller;
    [Header("Standard Ability Features")]
    public float AbilityDuration = 0f;
    public float AbilityCoolDown = 0f;
    public float TimeSinceLastUse = 100f;

    /// <summary>
    /// Happens all the time
    /// </summary>
    public virtual void PermanentUpdate()
    {
        if (TimeSinceLastUse <= AbilityDuration && TimeSinceLastUse + Time.deltaTime > AbilityDuration)
        {
            AbilityEnd();
            CooldownStart();
        }

        if (TimeSinceLastUse <= AbilityDuration + AbilityCoolDown && TimeSinceLastUse + Time.deltaTime > AbilityDuration + AbilityCoolDown)
        {
            CoolDownEnd();
        }

        TimeSinceLastUse += Time.deltaTime;
    }

    /// <summary>
    /// Starts the ability, if it can be started
    /// </summary>
    [ContextMenu("Try Start Ability")]
    public virtual void TryAbltiyStart()
    {
        if (TimeSinceLastUse >= AbilityDuration + AbilityCoolDown)
        {
            TimeSinceLastUse = 0;
            AbilityStart();
        }
    }

    /// <summary>
    /// Happens at the start of the ability
    /// </summary>
    public virtual void AbilityStart()
    {

    }

    /// <summary>
    /// What happens during the ability
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

    /// <summary>
    /// What happens at the start of the cooldown phase
    /// </summary>
    public virtual void CooldownStart()
    {

    }

    /// <summary>
    /// Happens during the entire cooldown
    /// </summary>
    public virtual void CoolDownUpdate()
    {

    }

    /// <summary>
    /// Happens at the end of the cooldown
    /// </summary>
    public virtual void CoolDownEnd()
    {

    }
}
