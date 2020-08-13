using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Ammo : MonoBehaviour
{
    [Range(1, 200)] public int Amount = 10;
    [SerializeField] private Vector3 rotationVector = Vector3.zero;
    [SerializeField] protected VisualEffect onPickUpEffect;
    [SerializeField] protected bool repositionsAtStart = true;

    public void Start()
    {
        RaycastHit hit;
        if (repositionsAtStart && Physics.Raycast(transform.position, Vector3.down, out hit, 5f))
        {
            transform.position = new Vector3(hit.point.x, hit.point.y + 0.1f, hit.point.z);
        }
    }

    public void Update()
    {
        transform.rotation = Quaternion.Euler(rotationVector * Time.deltaTime + transform.rotation.eulerAngles);
    }

    public void OnDeath()
    {
        if (onPickUpEffect != null)
        {
            Instantiate(onPickUpEffect, transform.position, transform.rotation, GameManager.Instance.ParticleHolder);
        }
        Destroy(gameObject);
    }
}