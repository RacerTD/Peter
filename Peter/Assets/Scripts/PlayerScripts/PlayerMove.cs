using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : Ability
{
    [Header("Custom Ability Features")]

    public float MoveSpeed = 2f;
    public float SprintSpeed = 4f;
    public float SneakSpeed = 1f;

    #region WalkingType
    private WalkingType walkingType = WalkingType.Normal;
    public WalkingType WalkingType
    {
        get => walkingType;
        set
        {
            walkingType = value;
            UpdateWalkingType(value);
        }
    }

    private void UpdateWalkingType(WalkingType value)
    {
        switch (value)
        {
            case WalkingType.Normal:
                currentSpeed = MoveSpeed;
                break;
            case WalkingType.Sprint:
                currentSpeed = SprintSpeed;
                break;
            case WalkingType.Sneak:
                currentSpeed = SneakSpeed;
                break;
        }
    }

    #endregion

    private float currentSpeed = 0f;

    public void Start()
    {
        currentSpeed = MoveSpeed;
    }

    public override void AbilityUpdate()
    {
        if (currentInputAction.started || currentInputAction.performed)
        {
            transform.Translate(Vector3.RotateTowards(transform.position, new Vector3(currentInputAction.ReadValue<Vector2>().x * Time.deltaTime * currentSpeed, 0, currentInputAction.ReadValue<Vector2>().y * Time.deltaTime * currentSpeed), float.MaxValue, float.MaxValue));
        }

        base.AbilityUpdate();
    }
}

public enum WalkingType
{
    Sprint,
    Sneak,
    Normal
}
