using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public UnityEvent m_Interact = new UnityEvent();

    public void OnHit()
    {
        m_Interact.Invoke();
    }
}
