using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyController))]
public abstract class EnemyAbility : MonoBehaviour
{
    [Header("Standard Ability Features")]
    public float AbilityDuration = 0f;
    public float AbilityCoolDown = 0f;
    public float TimeSinceLastUse = 0f;

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
    /// Happens at the start of the ability
    /// </summary>
    public virtual void AbilityStart()
    {
        if (TimeSinceLastUse > AbilityDuration + AbilityCoolDown)
        {
            TimeSinceLastUse = 0f;
        }
        else
        {
            Debug.LogWarning("AI did something to early");
        }
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
