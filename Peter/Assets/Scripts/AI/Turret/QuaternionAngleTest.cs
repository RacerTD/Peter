using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuaternionAngleTest : MonoBehaviour
{
    public Vector3 Angle = new Vector3();

    private void Update()
    {
        Debug.Log(Quaternion.Angle(Quaternion.Euler(Vector3.zero), Quaternion.Euler(Angle)));
    }
    private void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position, Vector3.up, Color.red);
        Debug.DrawRay(transform.position, Angle.normalized, Color.red);
    }
}
