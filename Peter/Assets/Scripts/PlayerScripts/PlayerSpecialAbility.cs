using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpecialAbility : Ability
{
    public SpecialAbility CurrentSpecialAbility;

    public override void AbilityStart(InputAction.CallbackContext context)
    {
        if (CurrentSpecialAbility != null)
        {
            CurrentSpecialAbility.AbilityStart(context);
        }
        base.AbilityStart(context);
    }

    public override void AbilityUpdate()
    {
        if (CurrentSpecialAbility != null)
        {
            CurrentSpecialAbility.AbilityUpdate();
        }
    }

    public override void AbilityEnd()
    {
        if (CurrentSpecialAbility != null)
        {
            CurrentSpecialAbility.AbilityEnd();
        }
    }

    public override void AbilityCoolDownStart()
    {
        if (CurrentSpecialAbility != null)
        {
            CurrentSpecialAbility.AbilityCoolDownStart();
        }
    }

    public override void AbilityCoolDownUpdate()
    {
        if (CurrentSpecialAbility != null)
        {
            CurrentSpecialAbility.AbilityCoolDownUpdate();
        }
    }

    public override void AbilityCoolDownEnd()
    {
        if (CurrentSpecialAbility != null)
        {
            CurrentSpecialAbility.AbilityCoolDownEnd();
        }
    }
}
