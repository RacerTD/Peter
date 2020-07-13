using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : Ability
{
    [Header("Custom Ability Features")]
    [SerializeField] private float lookSensitivity = 0.3f;
    [SerializeField] protected Camera controlledCamera;
    [SerializeField] protected float recoverySpeed = 2f;
    private Vector2 lookVecor = Vector2.zero;
    private Vector3 shouldDirection = Vector3.zero;
    private Vector3 directionOffset = Vector3.zero;
    private Recoil rec = new Recoil();

    private void Start()
    {
        shouldDirection = controlledCamera.transform.localRotation.eulerAngles;
    }

    public override void AbilityUpdate()
    {
        transform.rotation = Quaternion.Euler(new Vector3(0f, currentInputAction.ReadValue<Vector2>().x * lookSensitivity + transform.rotation.eulerAngles.y, 0));

        CalculateShouldDirection();

        controlledCamera.transform.localRotation = Quaternion.Euler(shouldDirection + directionOffset);

        CalculateOffset();

        Debug.DrawRay(controlledCamera.transform.position, controlledCamera.transform.forward, Color.red);

        base.AbilityUpdate();
    }

    private void CalculateShouldDirection()
    {
        shouldDirection = new Vector3(-currentInputAction.ReadValue<Vector2>().y * lookSensitivity + shouldDirection.x, 0f, 0f);

        if (shouldDirection.x > 85f && shouldDirection.x < 275f)
        {
            if (shouldDirection.x <= 180)
            {
                shouldDirection = new Vector3(85f, 0f, 0);
            }
            else if (shouldDirection.x > 180)
            {
                shouldDirection = new Vector3(275f, 0f, 0);
            }
        }
    }

    private void CalculateOffset()
    {
        if (rec.time > 0)
        {
            directionOffset += Vector3.Lerp(Vector3.zero, rec.amount, rec.time);
            rec.time -= Time.deltaTime;
        }
        else
        {
            directionOffset -= directionOffset * recoverySpeed * Time.deltaTime;
        }
    }

    public void AddOffset(Vector3 amount, float time)
    {
        rec.amount = amount;
        rec.time = time;
    }

    private struct Recoil
    {
        public Vector3 amount;
        public float time;
    }
}
