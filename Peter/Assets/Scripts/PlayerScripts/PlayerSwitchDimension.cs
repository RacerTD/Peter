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
    public bool DimA = true;

    public override void AbilityUpdate()
    {
        if (!dimensionSwitched)
        {
            if (DimA)
            {
                transform.position += DimensionOffset;
            }
            else
            {
                transform.position -= DimensionOffset;
            }

            dimensionSwitched = true;
            DimA = !DimA;
        }

        base.AbilityUpdate();
    }

    public override void AbilityEnd()
    {
        dimensionSwitched = false;
        base.AbilityEnd();
    }
}
