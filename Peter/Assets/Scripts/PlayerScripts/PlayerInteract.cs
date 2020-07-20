using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInteract : Ability
{
    [Header("Custom Ability Features")]
    [SerializeField] private float interactionDistance = 4f;
    public override void AbilityStart()
    {
        RaycastHit[] hits = Physics.RaycastAll(Camera.main.transform.position, Camera.main.transform.forward, interactionDistance);
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.GetComponent<Interactable>() != null)
            {
                hit.collider.GetComponent<Interactable>().OnHit();
                return;
            }
            else if (hit.collider.GetComponentInParent<Interactable>() != null)
            {
                hit.collider.GetComponentInParent<Interactable>().OnHit();
                return;
            }
        }
    }
}
