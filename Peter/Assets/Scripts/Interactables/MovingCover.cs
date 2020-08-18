using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingCover : MonoBehaviour
{
    [SerializeField] private float timeToDeploy = 0f;
    [SerializeField] [Tooltip("If 0 this is deactivated")] private float timeDeployed = 0f;
    private float timeDeplayedTimer = 0f;
    [SerializeField] private float timeTillDeploy = 0f;
    private float timeTillDeploedTimer = 0f;
    [SerializeField] private CoverState coverState = CoverState.InActive;
    private Vector3 inActivePosition = Vector3.zero;
    [SerializeField] private List<MovingObject> movingObjects = new List<MovingObject>();

    private void Update()
    {
        if (movingObjects.Count > 0)
        {
            switch (coverState)
            {
                case CoverState.Active:
                    for (int i = 0; i <= movingObjects.Count; i++)
                    {
                        movingObjects[i].Object.localPosition = Vector3.MoveTowards(movingObjects[i].Object.localPosition, movingObjects[i].PositionToMoveTo, movingObjects[i].PositionToMoveTo.magnitude / timeToDeploy * Time.deltaTime);
                    }
                    if (timeDeployed > 0f)
                    {
                        timeDeplayedTimer -= Time.deltaTime;
                        if (timeDeplayedTimer <= 0f)
                        {
                            DeactivateCover();
                        }
                    }
                    break;
                case CoverState.InActive:
                    for (int i = 0; i <= movingObjects.Count; i++)
                    {
                        movingObjects[i].Object.localPosition = Vector3.MoveTowards(movingObjects[i].Object.localPosition, movingObjects[i].InActivePosition, movingObjects[i].PositionToMoveTo.magnitude / timeToDeploy * Time.deltaTime);
                    }
                    break;
                case CoverState.Locked:
                    break;
                case CoverState.WaitingToDeploy:
                    timeTillDeploedTimer -= Time.deltaTime;
                    if (timeTillDeploedTimer <= 0f)
                    {
                        coverState = CoverState.Active;
                    }
                    break;
                default:
                    Debug.LogError("And on that terrible disappointment, it's time to end.");
                    break;
            }
        }
    }

    /// <summary>
    /// Opens or closes the door
    /// </summary>
    [ContextMenu("Activate or Deactivate")]
    public void ToggleCover()
    {
        switch (coverState)
        {
            case CoverState.Active:
                DeactivateCover();
                break;
            case CoverState.InActive:
                ActivateCover();
                break;
            case CoverState.Locked:
                break;
            case CoverState.WaitingToDeploy:
                break;
            default:
                Debug.LogError("A wizard is never late, nor is he early, he arrives precisely when he means to.");
                break;
        }
    }

    /// <summary>
    /// Activates the cover
    /// </summary>
    public void ActivateCover()
    {
        coverState = CoverState.WaitingToDeploy;
        timeDeplayedTimer = timeDeployed;
        if (timeTillDeploedTimer > 0 && timeTillDeploedTimer < timeTillDeploy)
        {
            timeTillDeploedTimer = timeTillDeploy;
        }
    }

    /// <summary>
    /// Deactivates the cover
    /// </summary>
    public void DeactivateCover()
    {
        coverState = CoverState.InActive;
    }

    [System.Serializable]
    public struct MovingObject
    {
        public Transform Object;
        public Vector3 PositionToMoveTo;
        public Vector3 InActivePosition;
    }

    private enum CoverState
    {
        Active,
        InActive,
        Locked,
        WaitingToDeploy
    }

    private void OnDrawGizmosSelected()
    {
        if (movingObjects.Count > 0)
        {
            foreach (MovingObject obj in movingObjects)
            {
                Debug.DrawLine(obj.Object.position + obj.InActivePosition, obj.Object.position + obj.PositionToMoveTo, Color.blue);
            }
        }
    }
}
