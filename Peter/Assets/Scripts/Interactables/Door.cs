using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Door : MonoBehaviour
{
    [SerializeField] private float timeToOpen = 0f;
    private float degreesPerSecond = 0f;
    private bool isOpen = false;
    private Vector3 closedRotation = Vector3.zero;
    [SerializeField] private Vector3 openRotation = Vector3.zero;
    [SerializeField] private List<Transform> thingsToOpen = new List<Transform>();

    private void Start()
    {
        degreesPerSecond = openRotation.y / timeToOpen;
    }

    private void Update()
    {
        if (isOpen)
        {
            foreach (Transform part in thingsToOpen)
            {
                part.rotation = Quaternion.RotateTowards(part.rotation, Quaternion.Euler(openRotation), degreesPerSecond * Time.deltaTime);
            }
        }
        else
        {
            foreach (Transform part in thingsToOpen)
            {
                part.rotation = Quaternion.RotateTowards(part.rotation, Quaternion.Euler(closedRotation), degreesPerSecond * Time.deltaTime);
            }
        }
    }

    public void OpenCloseDoor()
    {
        isOpen = !isOpen;
    }

    public void OpenDoor()
    {
        isOpen = true;
    }

    public void CloseDoor()
    {
        isOpen = false;
    }

    [ContextMenu("Open or Close")]
    public void OpenDoorInEditor()
    {
        if (isOpen)
            foreach (Transform part in thingsToOpen)
                part.rotation = Quaternion.Euler(openRotation);
        else
            foreach (Transform part in thingsToOpen)
                part.rotation = Quaternion.Euler(closedRotation);

        OpenCloseDoor();
    }
}
