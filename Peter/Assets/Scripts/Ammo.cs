using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    [Range(1, 200)]
    public int Amount = 10;
    public AmmoType Type = AmmoType.DimA;
    [SerializeField]
    private Vector3 rotationVector = Vector3.zero;

    public void Start()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 5f))
        {
            transform.position = new Vector3(hit.point.x, hit.point.y + 0.1f, hit.point.z);
        }
    }

    public void Update()
    {
        transform.rotation = Quaternion.Euler(rotationVector * Time.deltaTime + transform.rotation.eulerAngles);
    }
}

public enum AmmoType
{
    DimA,
    DimB
}