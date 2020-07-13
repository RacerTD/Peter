using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRecoil : MonoBehaviour
{
    [SerializeField] protected Transform camTransform;
    private Vector3 camStartRot;
    [SerializeField] [Range(0, 4)] protected float recoverSpeed = 0.03f;
    public float WeaponRecoil = 10f;

    private void Start()
    {
        camStartRot = camTransform.rotation.eulerAngles;
    }

    private void Update()
    {
        camTransform.rotation = Quaternion.Euler((camTransform.rotation.eulerAngles - camStartRot) * (recoverSpeed * Time.deltaTime));
    }

    public void DoRecoil(float amount)
    {
        camTransform.rotation = Quaternion.Euler(new Vector3(Random.Range(0, amount * 2), Random.Range(-amount, amount), 0) + camStartRot);
    }
}
