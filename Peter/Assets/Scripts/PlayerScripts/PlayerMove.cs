using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMove : Ability
{
    [Header("Custom Ability Features")]
    [SerializeField] private Vector3 moveVector = Vector3.zero;
    [SerializeField] private Vector3 shouldVector = Vector3.zero;
    [SerializeField] protected float accelerationSpeed = 0.1f;
    public float MoveSpeed = 2f;

    [Header("Sprint")]
    public float SprintSpeed = 4f;
    [SerializeField] private bool sprintIsToggle = false;

    [Header("Sneak")]
    public float SneakSpeed = 1f;
    [SerializeField] private bool sneakIsToggle = false;

    [Header("Jump")]
    [SerializeField] private Vector3 jumpForce = Vector3.zero;
    public WalkingType WalkingType = WalkingType.Normal;
    private Rigidbody physicsbody;

    protected override void Start()
    {
        physicsbody = GetComponent<Rigidbody>();
        base.Start();
    }

    /// <summary>
    /// Action when the sprint button is pressed
    /// </summary>
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

    /// <summary>
    /// Action when the sneak button is pressed
    /// </summary>
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

    /// <summary>
    /// Action when the jump button is pressed
    /// </summary>
    public void DoJump(InputAction.CallbackContext context)
    {
        GetComponent<Rigidbody>().AddForce(jumpForce);
    }

    public override void AbilityStart(InputAction.CallbackContext context)
    {
        UpdateVelocity(context);

        base.AbilityStart(context);
    }

    public override void AbilityUpdate()
    {
        UpdateVelocity(currentInputAction);

        base.AbilityUpdate();
    }

    private void FixedUpdate()
    {
        moveVector = Vector3.Lerp(moveVector, shouldVector, accelerationSpeed * Time.fixedDeltaTime);
        Vector3 stuff = Vector3.RotateTowards(transform.position, moveVector, float.MaxValue, float.MaxValue);

        //physicsbody.MovePosition(transform.position += stuff * Time.fixedDeltaTime);

        physicsbody.velocity = new Vector3(stuff.x, physicsbody.velocity.y, stuff.z);
        Debug.Log($"{stuff} || {physicsbody.velocity}");
    }

    /// <summary>
    /// Not in use
    /// </summary>
    private void ApplyVelocity()
    {
        moveVector = Vector3.Lerp(moveVector, shouldVector, accelerationSpeed * Time.deltaTime);
        transform.Translate(Vector3.RotateTowards(transform.position, moveVector * Time.deltaTime, float.MaxValue, float.MaxValue));
    }

    /// <summary>
    /// Updates the current velocity the player has
    /// </summary>
    private void UpdateVelocity(InputAction.CallbackContext context)
    {
        if (context.started || context.performed)
        {
            switch (WalkingType)
            {
                case WalkingType.Normal:
                    shouldVector = new Vector3(context.ReadValue<Vector2>().x * MoveSpeed, 0, context.ReadValue<Vector2>().y * MoveSpeed);
                    break;
                case WalkingType.Sprint:
                    shouldVector = new Vector3(context.ReadValue<Vector2>().x * MoveSpeed, 0, Mathf.Clamp(context.ReadValue<Vector2>().y * SprintSpeed, -MoveSpeed, SprintSpeed));
                    break;
                case WalkingType.Sneak:
                    shouldVector = new Vector3(context.ReadValue<Vector2>().x * SneakSpeed, 0, context.ReadValue<Vector2>().y * SneakSpeed);
                    break;
            }
        }

        if (context.canceled)
        {
            shouldVector = Vector3.zero;
        }
    }
}

public enum WalkingType
{
    Sprint,
    Sneak,
    Normal
}
