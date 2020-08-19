using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireThing : MonoBehaviour
{
    public float force = 10f;
    public bool isActive = false;
    public float timeSinceActive = 0f;
    public float timeActive = 10f;
    protected Rigidbody body;
    private Vector3 forceDirection = Vector3.zero;

    private void Start()
    {
        body = GetComponent<Rigidbody>();
        forceDirection = new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), Random.Range(-100, 100)).normalized;
    }

    private void Update()
    {
        if (isActive)
        {
            timeSinceActive += Time.deltaTime;
            if (timeSinceActive <= timeActive)
            {
                body.AddForce(forceDirection * force * Time.deltaTime, ForceMode.Impulse);
            }
        }
    }

    public void HitThing()
    {
        isActive = true;
    }
}
