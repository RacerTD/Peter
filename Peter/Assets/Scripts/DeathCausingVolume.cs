using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DeathCausingVolume : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Damagable>() != null)
        {
            other.GetComponent<Damagable>().DoDamage(float.MaxValue);
        }
    }
}
