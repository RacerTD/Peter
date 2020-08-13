using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public UnityEvent m_Interact = new UnityEvent();
    [SerializeField] protected AudioClip onHitSound;

    /// <summary>
    /// Gets activated when someone trys to interact with the object
    /// </summary>
    public virtual void OnHit()
    {
        if (onHitSound != null)
        {
            AudioManager.Instance.PlayNewSound(AudioType.Sfx, onHitSound, gameObject);
        }
        m_Interact.Invoke();
    }
}
