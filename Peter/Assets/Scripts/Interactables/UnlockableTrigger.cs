﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnlockableTrigger : MonoBehaviour
{
    public UnityEvent m_OnTriggerEnter = new UnityEvent();
    public UnityEvent m_OnTriggerExit = new UnityEvent();
    public bool Locked = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>() != null && !Locked)
        {
            m_OnTriggerEnter.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Player>() != null && !Locked)
        {
            m_OnTriggerExit.Invoke();
        }
    }

    public void Unlock()
    {
        Locked = false;
    }
}
