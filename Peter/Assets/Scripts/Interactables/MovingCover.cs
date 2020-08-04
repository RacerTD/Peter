using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MovingCover : MonoBehaviour
{
    [SerializeField] private float timeToDeploy = 0f;
    [SerializeField] [Tooltip("If 0 this is deactivated")] private float timeDeployed = 0f;
    private float timeDeplayedTimer = 0f;
    [SerializeField] private float timeTillDeploy = 0f;
    private float timeTillDeploedTimer = 0f;
    private float metersPerSecond = 0f;
    [SerializeField] private CoverState coverState = CoverState.InActive;
    private Vector3 inActivePosition = Vector3.zero;
    [SerializeField] private Vector3 activePosition = Vector3.zero;
    [SerializeField] protected Transform cover;

    private void Start()
    {
        metersPerSecond = activePosition.magnitude / timeToDeploy;
    }

    private void Update()
    {
        if (cover != null)
        {
            switch (coverState)
            {
                case CoverState.Active:
                    cover.localPosition = Vector3.MoveTowards(cover.localPosition, activePosition, metersPerSecond * Time.deltaTime);
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
                    cover.localPosition = Vector3.MoveTowards(cover.localPosition, inActivePosition, metersPerSecond * Time.deltaTime);
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
        timeTillDeploedTimer = timeTillDeploy;
    }

    /// <summary>
    /// Deactivates the cover
    /// </summary>
    public void DeactivateCover()
    {
        coverState = CoverState.InActive;
    }

    private enum CoverState
    {
        Active,
        InActive,
        Locked,
        WaitingToDeploy
    }
}
