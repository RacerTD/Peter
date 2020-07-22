using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretRotator : MonoBehaviour
{
    [HideInInspector] public Vector3 ShouldVector = Vector3.zero;
    [HideInInspector] public Transform RotatingThing;
    [HideInInspector] public float RotationSpeed = 20f;

    private void Update()
    {
        RotatingThing.rotation = Quaternion.RotateTowards(RotatingThing.rotation, Quaternion.Euler(ShouldVector), RotationSpeed * Time.deltaTime);
    }
}
