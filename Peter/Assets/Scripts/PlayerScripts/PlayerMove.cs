using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMove : PlayerAbility
{
    [Header("Custom Ability Features")]
    public Vector3 MoveVector = Vector3.zero;
    public Vector3 ShouldVector = Vector3.zero;
    [SerializeField] protected float accelerationSpeed = 0.1f;
    public float MoveSpeed = 2f;

    [Header("Sprint")]
    public float SprintSpeed = 4f;
    [SerializeField] private bool sprintIsToggle = false;

    [Header("Crouch")]
    public float CrouchSpeed = 1f;
    public float CrouchTime = 1f;
    private float timeSinceCrouch = 0f;
    private float timeSinceNoCrouch = 0f;
    [SerializeField] private float crouchHeight = 1f;
    private float normalLocalCameraHeight = 0f;
    private float crouchLocalCameraHeight = 0.8f;
    private float normalColHeight = 0f;
    [SerializeField] private bool sneakIsToggle = false;

    [Header("Jump")]
    [SerializeField] private Vector3 jumpForce = Vector3.zero;
    private WalkingType walkingType = WalkingType.Normal;
    public WalkingType WalkingType
    {
        get => walkingType;
        set
        {
            walkingType = value;
        }
    }

    private Rigidbody physicsbody;
    private CapsuleCollider capCol;
    private PlayerShoot playerShoot;
    private Camera cam;
    private Vector2 inputValue = Vector2.zero;

    protected override void Start()
    {
        physicsbody = GetComponent<Rigidbody>();
        playerShoot = GetComponent<PlayerShoot>();
        capCol = GetComponent<CapsuleCollider>();
        normalColHeight = capCol.height;
        cam = GetComponentInChildren<Camera>();
        normalLocalCameraHeight = cam.transform.localPosition.y;
        crouchLocalCameraHeight = capCol.center.y + crouchHeight / 2;
        base.Start();
    }

    /// <summary>
    /// Action when the sprint button is pressed
    /// </summary>
    public void ToggleSprint(InputAction.CallbackContext context)
    {
        if (!AbilityBlocked())
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
    }

    /// <summary>
    /// Action when the sneak button is pressed
    /// </summary>
    public void ToggleCrouch(InputAction.CallbackContext context)
    {
        if (!AbilityBlocked())
        {
            if (sneakIsToggle)
            {
                if (WalkingType == WalkingType.Crouch)
                {
                    WalkingType = WalkingType.Normal;
                }
                else
                {
                    WalkingType = WalkingType.Crouch;
                }
            }
            else
            {
                if (context.started)
                {
                    WalkingType = WalkingType.Crouch;
                }
                else if (context.canceled)
                {
                    WalkingType = WalkingType.Normal;
                }
            }
        }
    }

    /// <summary>
    /// Action when the jump button is pressed
    /// </summary>
    public void DoJump(InputAction.CallbackContext context)
    {
        if (!AbilityBlocked())
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position + new Vector3(0, 0.05f, 0), Vector3.down, out hit, 2f))
            {
                if (context.started && Vector3.Distance(transform.position + new Vector3(0, 0.05f, 0), hit.point) <= 0.1f)
                {
                    physicsbody.AddForce(jumpForce);
                }
            }
        }
    }

    public override void GetInput(InputAction.CallbackContext context)
    {
        inputValue = context.canceled ? Vector2.zero : context.ReadValue<Vector2>();
        base.GetInput(context);
    }

    public override void PermanentUpdate()
    {
        UpdateAnimation();
    }

    public override void AbilityUpdate()
    {
        UpdateVelocity();

        base.AbilityUpdate();
    }

    /// <summary>
    /// Updates the animation the gun is using
    /// </summary>
    private void UpdateAnimation()
    {
        if (ShouldVector.magnitude != 0f)
        {
            switch (WalkingType)
            {
                case WalkingType.Crouch:
                case WalkingType.Normal:
                    player.AddAnimState(WeaponAnimationState.Walking, Time.deltaTime * 2);
                    break;
                case WalkingType.Sprint:
                    player.AddAnimState(WeaponAnimationState.Running, Time.deltaTime * 2);
                    break;
                default:
                    break;
            }
        }
    }

    private void FixedUpdate()
    {
        UpdateCrouch();
        transform.Translate(Vector3.RotateTowards(transform.position, MoveVector * Time.fixedDeltaTime, float.MaxValue, float.MaxValue));
    }

    /// <summary>
    /// Updates the current velocity the player has
    /// </summary>
    private void UpdateVelocity()
    {
        if (InputStarted || InputPerformed && GameManager.Instance.CurrentGameState != GameState.Dead)
        {
            switch (WalkingType)
            {
                case WalkingType.Normal:
                    ShouldVector = new Vector3(inputValue.x * MoveSpeed, 0, inputValue.y * MoveSpeed);
                    break;
                case WalkingType.Sprint:
                    ShouldVector = new Vector3(inputValue.x * MoveSpeed, 0, Mathf.Clamp(inputValue.y * SprintSpeed, -MoveSpeed, SprintSpeed));
                    break;
                case WalkingType.Crouch:
                    ShouldVector = new Vector3(inputValue.x * CrouchSpeed, 0, inputValue.y * CrouchSpeed);
                    break;
                default:
                    Debug.LogWarning($"You shall not pass!");
                    break;
            }
        }

        if (InputCanceled)
        {
            ShouldVector = Vector3.zero;
        }

        MoveVector = Vector3.Lerp(MoveVector, ShouldVector, accelerationSpeed * Time.deltaTime);
    }

    private void UpdateCrouch()
    {
        switch (WalkingType)
        {
            case WalkingType.Normal:
            case WalkingType.Sprint:
                capCol.height = Mathf.Lerp(crouchHeight, normalColHeight, timeSinceNoCrouch / CrouchTime);
                cam.transform.localPosition = new Vector3(0, Mathf.Lerp(crouchLocalCameraHeight, normalLocalCameraHeight, timeSinceNoCrouch / CrouchTime), 0);
                timeSinceCrouch = 0f;
                timeSinceNoCrouch += Time.fixedDeltaTime;
                break;
            case WalkingType.Crouch:
                capCol.height = Mathf.Lerp(normalColHeight, crouchHeight, timeSinceCrouch / CrouchTime);
                cam.transform.localPosition = new Vector3(0, Mathf.Lerp(normalLocalCameraHeight, crouchLocalCameraHeight, timeSinceCrouch / CrouchTime), 0);
                timeSinceCrouch += Time.fixedDeltaTime;
                timeSinceNoCrouch = 0f;
                break;
            default:
                Debug.LogWarning($"A by-product of the TV industry");
                break;
        }
    }
}

public enum WalkingType
{
    Sprint,
    Crouch,
    Normal
}
