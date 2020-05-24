using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSwitchDimension : Ability
{
    private bool dimensionSwitched = false;
    private bool dimOne = true;

    public override void AbilityUpdate()
    {
        if (!dimensionSwitched)
        {
            if (dimOne)
            {
                transform.position += Vector3.up * 100f;
            }
            else
            {
                transform.position += Vector3.up * -100f;
            }

            dimensionSwitched = true;
            dimOne = !dimOne;
        }

        base.AbilityUpdate();
    }

    public override void AbilityEnd()
    {
        dimensionSwitched = false;
        base.AbilityEnd();
    }
}
