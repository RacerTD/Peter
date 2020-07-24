using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public UnityEvent m_Interact = new UnityEvent();

    /// <summary>
    /// Gets activated when someone trys to interact with the object
    /// </summary>
    public virtual void OnHit()
    {
        m_Interact.Invoke();
    }
}
