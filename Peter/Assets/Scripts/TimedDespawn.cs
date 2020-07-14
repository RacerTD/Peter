using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedDespawn : MonoBehaviour
{
    [SerializeField] protected float timeTillDespawn = 10f;

    /// <summary>
    /// Sets the time till despawn
    /// </summary>
    public void SetUpTimedDespawn(float time)
    {
        timeTillDespawn = time;
    }

    public void Update()
    {
        timeTillDespawn -= Time.deltaTime;
        if (timeTillDespawn <= 0)
        {
            Destroy(gameObject);
        }
    }
}
