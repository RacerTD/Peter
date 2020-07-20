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
    private Vector2 inputValue = Vector2.zero;

    public override void GetInput(InputAction.CallbackContext context)
    {
        inputValue = context.canceled ? Vector2.zero : context.ReadValue<Vector2>();
        base.GetInput(context);
    }

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
        if (sprintIsToggle && context.started)
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
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 2f))
        {
            if (context.started && Vector3.Distance(transform.position, hit.point) <= 0.05f)
            {
                physicsbody.AddForce(jumpForce);
            }
        }
    }

    public override void AbilityUpdate()
    {
        UpdateVelocity();

        base.AbilityUpdate();
    }

    private void FixedUpdate()
    {
        transform.Translate(Vector3.RotateTowards(transform.position, moveVector * Time.fixedDeltaTime, float.MaxValue, float.MaxValue));
    }

    /// <summary>
    /// Updates the current velocity the player has
    /// </summary>
    private void UpdateVelocity()
    {
        if (InputStarted || InputPerformed)
        {
            switch (WalkingType)
            {
                case WalkingType.Normal:
                    shouldVector = new Vector3(inputValue.x * MoveSpeed, 0, inputValue.y * MoveSpeed);
                    break;
                case WalkingType.Sprint:
                    shouldVector = new Vector3(inputValue.x * MoveSpeed, 0, Mathf.Clamp(inputValue.y * SprintSpeed, -MoveSpeed, SprintSpeed));
                    break;
                case WalkingType.Sneak:
                    shouldVector = new Vector3(inputValue.x * SneakSpeed, 0, inputValue.y * SneakSpeed);
                    break;
            }
        }

        if (InputCanceled)
        {
            shouldVector = Vector3.zero;
        }

        moveVector = Vector3.Lerp(moveVector, shouldVector, accelerationSpeed * Time.deltaTime);
    }
}

public enum WalkingType
{
    Sprint,
    Sneak,
    Normal
}
