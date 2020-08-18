using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationSetter : MonoBehaviour
{
    [SerializeField] private Transform objectToRotate;
    [SerializeField] private Vector3 rotationToSetTo = Vector3.zero;
    private bool gotSet = false;
    public void SetRotation()
    {
        if (!gotSet)
        {
            objectToRotate.rotation = Quaternion.Euler(rotationToSetTo);
            gotSet = true;
        }
    }
}
