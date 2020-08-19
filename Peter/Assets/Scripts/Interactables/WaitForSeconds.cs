using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WaitForSeconds : MonoBehaviour
{
    [SerializeField] private UnityEvent m_Event = new UnityEvent();
    [SerializeField] private float timeToWait = 10f;
    private float timeToWaitTimer = 0f;
    private bool isActive = false;

    private void Update()
    {
        if (isActive)
        {
            timeToWaitTimer += Time.deltaTime;
            if (timeToWaitTimer >= timeToWait)
            {
                isActive = false;
                m_Event.Invoke();
            }
        }
    }

    /// <summary>
    /// Activates the script
    /// </summary>
    public void Activate()
    {
        if (!isActive)
        {
            isActive = true;
            timeToWaitTimer = 0f;
        }
    }
}
