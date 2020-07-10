using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : Ability
{
    [Header("Custom Ability Features")]

    public float MoveSpeed = 2f;

    [Header("Sprint")]
    public float SprintSpeed = 4f;
    [SerializeField]
    private bool sprintIsToggle = false;

    [Header("Sneak")]
    public float SneakSpeed = 1f;
    [SerializeField]
    private bool sneakIsToggle = false;

    private float currentSpeed = 0f;

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

    public void Start()
    {
        currentSpeed = MoveSpeed;
    }

    public void ToggleSprint(InputAction.CallbackContext context)
    {
        if (sprintIsToggle)
        {
            if (WalkingType == WalkingType.Sprint)
            {
                WalkingType = WalkingType.Normal;
            }
            else
            {
                WalkingType = WalkingType.Sprint;
            }
        }
        else
        {
            if (context.started)
            {
                WalkingType = WalkingType.Sprint;
            }
            else if (context.canceled)
            {
                WalkingType = WalkingType.Normal;
            }
        }
    }

    public void ToggleSneak(InputAction.CallbackContext context)
    {
        if (sneakIsToggle)
        {
            if (WalkingType == WalkingType.Sneak)
            {
                WalkingType = WalkingType.Normal;
            }
            else
            {
                WalkingType = WalkingType.Sneak;
            }
        }
        else
        {
            if (context.started)
            {
                WalkingType = WalkingType.Sneak;
            }
            else if (context.canceled)
            {
                WalkingType = WalkingType.Normal;
            }
        }
    }

    public override void AbilityStart(InputAction.CallbackContext context)
    {
        //Debug.Log($"Got new Input {context.started}, {context.performed}, {context.canceled}");
        base.AbilityStart(context);
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
