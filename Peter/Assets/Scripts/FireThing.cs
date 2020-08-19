using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireThing : MonoBehaviour
{
    public Vector3 force = Vector3.up;
    public bool isActive = false;
    public float timeSinceActive = 0f;
    public float timeActive = 10f;
    protected Rigidbody body;
    private Vector3 forceDirection = Vector3.zero;

    private void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (isActive)
        {
            timeSinceActive += Time.deltaTime;
            if (timeSinceActive <= timeActive)
            {
                body.AddForce(force * Time.deltaTime, ForceMode.Impulse);
            }
        }
    }

    public void HitThing()
    {
        isActive = true;
    }
}