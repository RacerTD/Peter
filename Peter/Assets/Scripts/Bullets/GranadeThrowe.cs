using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GranadeThrowe : MonoBehaviour
{
    public float TimeBetweenThrows = 2f;
    public float TimeBetweenThrowsTimer = 0f;
    public Granade Granade;
    public Vector3 GranadeDirection = Vector3.zero;

    private void Update()
    {
        TimeBetweenThrowsTimer += Time.deltaTime;

        if (TimeBetweenThrowsTimer >= TimeBetweenThrows)
        {
            TimeBetweenThrowsTimer = 0f;
            Instantiate(Granade, transform.position, Quaternion.identity).SetupBullet(GranadeDirection, 10f, 10f, 10);
        }
    }
}
