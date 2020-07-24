using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Door : MonoBehaviour
{
    [SerializeField] private float timeToOpen = 0f;
    private float degreesPerSecond = 0f;
    [SerializeField] private DoorState doorState = DoorState.Closed;
    private Vector3 closedRotation = Vector3.zero;
    [SerializeField] private Vector3 openRotation = Vector3.zero;
    [SerializeField] private Transform door;

    private void Start()
    {
        degreesPerSecond = openRotation.y / timeToOpen;
    }

    private void Update()
    {
        if (door != null)
        {
            switch (doorState)
            {
                case DoorState.Open:
                    door.rotation = Quaternion.RotateTowards(door.rotation, Quaternion.Euler(openRotation + transform.rotation.eulerAngles), degreesPerSecond * Time.deltaTime);
                    break;
                case DoorState.Closed:
                    door.rotation = Quaternion.RotateTowards(door.rotation, Quaternion.Euler(closedRotation + transform.rotation.eulerAngles), degreesPerSecond * Time.deltaTime);
                    break;
                case DoorState.OpenBackwards:
                    door.rotation = Quaternion.RotateTowards(door.rotation, Quaternion.Euler(-openRotation + transform.rotation.eulerAngles), degreesPerSecond * Time.deltaTime);
                    break;
                case DoorState.Locked:
                    break;
                default:
                    Debug.LogError("I have a bad feeling about this.");
                    break;
            }
        }
    }

    /// <summary>
    /// Opens or closes the door
    /// </summary>
    [ContextMenu("Open or Close")]
    public void OpenCloseDoor()
    {
        switch (doorState)
        {
            case DoorState.Open:
                doorState = DoorState.Closed;
                break;
            case DoorState.Closed:
                if (CheckIfPlayerFrontBack())
                    doorState = DoorState.Open;
                else
                    doorState = DoorState.OpenBackwards;
                break;
            case DoorState.OpenBackwards:
                doorState = DoorState.Closed;
                break;
            case DoorState.Locked:
                break;
            default:
                Debug.LogError("I have a bad feeling about this.");
                break;
        }
    }

    /// <summary>
    /// Opens the door
    /// </summary>
    public void OpenDoor()
    {
        doorState = DoorState.Open;
    }

    /// <summary>
    /// Opens the door backwards
    /// </summary>
    public void OpenDoorBackwards()
    {
        doorState = DoorState.OpenBackwards;
    }

    /// <summary>
    /// Closes the door
    /// </summary>
    public void CloseDoor()
    {
        doorState = DoorState.Closed;
    }

    /// <summary>
    /// Checks if the player is in front or behind the door
    /// </summary>
    public bool CheckIfPlayerFrontBack()
    {
        return Vector3.Dot(GameManager.Instance.CurrentPlayer.transform.position - transform.position, transform.forward) <= 0f;
    }

    private enum DoorState
    {
        Open,
        Closed,
        OpenBackwards,
        Locked
    }

}
