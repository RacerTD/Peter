using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSwitchDimension : Ability
{
    [Header("Custom Ability Features")]
    [SerializeField]
    protected Vector3 DimensionOffset = Vector3.zero;
    private bool dimensionSwitched = false;
    private bool dimOne = true;

    public override void AbilityUpdate()
    {
        if (!dimensionSwitched)
        {
            if (dimOne)
            {
                transform.position += DimensionOffset;
            }
            else
            {
                transform.position -= DimensionOffset;
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
