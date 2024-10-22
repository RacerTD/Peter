﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerShoot))]
public class PlayerLook : PlayerAbility
{
    [Header("Custom Ability Features")]
    [SerializeField] private float mouseLookSensitivity = 0.3f;
    [SerializeField] private float controllerLookSensitivity = 0.3f;
    private float lookSensitivity = 0.3f;
    [SerializeField] protected Camera controlledCamera;
    [SerializeField] protected float recoverySpeed = 2f;
    private Vector2 lookVector = Vector2.zero;
    private Vector3 shouldDirection = Vector3.zero;
    private Vector3 directionOffset = Vector3.zero;
    private Recoil rec = new Recoil();
    private PlayerShoot playerShoot;
    [HideInInspector] public Vector2 InputValue;

    protected override void Start()
    {
        lookSensitivity = mouseLookSensitivity;
        shouldDirection = controlledCamera.transform.localRotation.eulerAngles;
        playerShoot = GetComponent<PlayerShoot>();
        base.Start();
    }

    public override void GetInput(InputAction.CallbackContext context)
    {
        InputValue = context.ReadValue<Vector2>();
        base.GetInput(context);
    }

    public override void AbilityUpdate()
    {
        if (!InputCanceled)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0f, InputValue.x * (playerShoot.isAiming ? lookSensitivity * playerShoot.Gun.ScopeLookSpeed : lookSensitivity) + transform.rotation.eulerAngles.y, 0));
            CalculateShouldDirection();
        }

        controlledCamera.transform.localRotation = Quaternion.Euler(shouldDirection + directionOffset);

        CalculateOffset();

        Debug.DrawRay(controlledCamera.transform.position, controlledCamera.transform.forward, Color.red);

        base.AbilityUpdate();
    }

    /// <summary>
    /// Calculates the direction the player should be looking
    /// </summary>
    private void CalculateShouldDirection()
    {
        shouldDirection = new Vector3(Mathf.Clamp(-InputValue.y * (playerShoot.isAiming ? lookSensitivity * playerShoot.Gun.ScopeLookSpeed : lookSensitivity) + shouldDirection.x, -85f, 85f), 0f, 0f);
    }

    /// <summary>
    /// Calculates the current recoil offset
    /// </summary>
    private void CalculateOffset()
    {
        if (directionOffset.magnitude >= rec.Max * 0.8f)
        {
            rec.Amount = new Vector3(rec.Amount.x * (rec.Max - directionOffset.magnitude), rec.Amount.y, rec.Amount.z);
        }

        if (rec.Time > 0)
        {
            directionOffset += Vector3.Lerp(Vector3.zero, rec.Amount, rec.Time);
            rec.Time -= Time.deltaTime;
        }
        else
        {
            directionOffset -= directionOffset * recoverySpeed * Time.deltaTime;
        }
    }

    /// <summary>
    /// Adds an offset to the player camera
    /// </summary>
    public void AddOffset(Vector3 amount, float time, float max)
    {
        rec.Amount = amount;
        rec.Time = time;
        rec.Max = max;
    }

    private struct Recoil
    {
        public Vector3 Amount;
        public float Time;
        public float Max;
    }

    public override void OnDeviceChanged(PlayerInput input)
    {
        switch (input.currentControlScheme)
        {
            case "Gamepad":
                lookSensitivity = controllerLookSensitivity;
                break;
            case "Keyboard&Mouse":
                lookSensitivity = mouseLookSensitivity;
                break;
        }

        Debug.Log(input.currentControlScheme);

        base.OnDeviceChanged(input);
    }
}
