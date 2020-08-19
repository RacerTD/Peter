using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveAndDestroy : MonoBehaviour
{
    public float timeTillDissolve = 10f;
    public float timeForDissolve = 1f;
    private float timeSinceStart = 0f;
    protected MeshRenderer rend;

    private void Start()
    {
        rend = GetComponent<MeshRenderer>();
        gameObject.AddComponent<TimedDespawn>().SetUpTimedDespawn((timeTillDissolve + timeForDissolve) * 1.1f);
    }

    private void Update()
    {
        timeSinceStart += Time.deltaTime;

        if (timeSinceStart >= timeTillDissolve)
        {
            foreach (Material mat in rend.materials)
            {
                mat.SetFloat("DissolveFactor", Mathf.Lerp(0, 1, (timeSinceStart - timeTillDissolve) / timeForDissolve));
            }
        }
    }
}
