using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedDespawn : MonoBehaviour
{
    [SerializeField]
    protected float timeTillDespawn = 10f;

    public void Update()
    {
        timeTillDespawn -= Time.deltaTime;
        if (timeTillDespawn <= 0)
        {
            Destroy(gameObject);
        }
    }
}
