using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class ShootButton : MonoBehaviour
{
    public UnityEvent m_OnHit = new UnityEvent();

    /// <summary>
    /// Get activated when a player bullet hits the button
    /// </summary>
    public void OnHit()
    {
        m_OnHit.Invoke();
    }
}
