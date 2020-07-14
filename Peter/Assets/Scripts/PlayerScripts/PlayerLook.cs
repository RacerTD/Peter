﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerShoot))]
public class PlayerLook : Ability
{
    [Header("Custom Ability Features")]
    [SerializeField] private float lookSensitivity = 0.3f;
    public Camera controlledCamera;
    [SerializeField] protected float recoverySpeed = 2f;
    private Vector2 lookVecor = Vector2.zero;
    private Vector3 shouldDirection = Vector3.zero;
    private Vector3 directionOffset = Vector3.zero;
    private Recoil rec = new Recoil();
    private PlayerShoot playerShoot;

    protected override void Start()
    {
        shouldDirection = controlledCamera.transform.localRotation.eulerAngles;
        playerShoot = GetComponent<PlayerShoot>();
        AbilityActive = true;
        base.Start();
    }

    public override void AbilityUpdate()
    {
        transform.rotation = Quaternion.Euler(new Vector3(0f, currentInputAction.ReadValue<Vector2>().x * (playerShoot.isAiming ? lookSensitivity * playerShoot.Gun.ScopeLookSpeed : lookSensitivity) + transform.rotation.eulerAngles.y, 0));

        CalculateShouldDirection();

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
        shouldDirection = new Vector3(Mathf.Clamp(-currentInputAction.ReadValue<Vector2>().y * (playerShoot.isAiming ? lookSensitivity * playerShoot.Gun.ScopeLookSpeed : lookSensitivity) + shouldDirection.x, -85f, 85f), 0f, 0f);
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
}
