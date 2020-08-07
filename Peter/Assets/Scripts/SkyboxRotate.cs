using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SkyboxRotate : MonoBehaviour
{
    public float RotationSpeedY = 1f;

    private void Update()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + RotationSpeedY * Time.deltaTime, transform.rotation.eulerAngles.z);
    }
}
