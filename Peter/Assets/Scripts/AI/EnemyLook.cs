using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLook : EnemyAbility
{
    public List<WaitingPoint> WaitingPoints = new List<WaitingPoint>();
    private int _waitingPointsIndex = 0;
    private int waitingPointsIndex
    {
        get => _waitingPointsIndex;
        set
        {
            if (value >= WaitingPoints.Count)
            {
                _waitingPointsIndex = 0;
            }
            else
            {
                _waitingPointsIndex = value;
            }
        }
    }
    private float waitTimeAtPos = 0f;

    public EnemyLookState LookState = EnemyLookState.Pacing;
    public Transform RotatingThing;
    public float RotationSpeed = 30f;
    public float RotationSpeedPlayerInSight = 60f;
    public float WaitTimeAfterLastSighting = 10f;
    public Vector3 ShouldVector = Vector3.zero;


    private void Start()
    {
        if (WaitingPoints.Count <= 0)
        {
            Debug.LogError($"Enemy has no default viewing direction");
            Debug.Break();
        }

        foreach (WaitingPoint point in WaitingPoints)
        {
            if (point.Direction == null)
            {
                Debug.LogError($"Positions to look at not set");
                Debug.Break();
            }
        }
    }

    public override void PermanentUpdate()
    {
        switch (LookState)
        {
            case EnemyLookState.Pacing:
                HandlePacing();
                break;
            case EnemyLookState.FollowPlayer:
                RotatingThing.rotation = Quaternion.RotateTowards(RotatingThing.rotation, Quaternion.LookRotation(Controller.LastSeenPlayerPosition - transform.position, Vector3.up), RotationSpeedPlayerInSight * Time.deltaTime);
                break;
            case EnemyLookState.Waiting:
                RotatingThing.rotation = Quaternion.RotateTowards(RotatingThing.rotation, Quaternion.LookRotation(Controller.LastSeenPlayerPosition - transform.position, Vector3.up), RotationSpeedPlayerInSight * Time.deltaTime);
                if (WaitTimeAfterLastSighting <= Controller.TimeSinceLastSighting)
                {
                    LookState = EnemyLookState.Pacing;
                }
                break;
            default:
                break;
        }
    }

    public void HandlePacing()
    {
        RotatingThing.rotation = Quaternion.RotateTowards(RotatingThing.rotation, Quaternion.LookRotation(WaitingPoints[waitingPointsIndex].Direction.position - transform.position), RotationSpeed * Time.deltaTime);

        if (RotatingThing.rotation == Quaternion.LookRotation(WaitingPoints[waitingPointsIndex].Direction.position - transform.position))
        {
            waitTimeAtPos += Time.deltaTime;
            if (waitTimeAtPos >= WaitingPoints[waitingPointsIndex].TimeToWait)
            {
                waitingPointsIndex++;
                waitTimeAtPos = 0f;
            }
        }
    }

    [System.Serializable]
    public struct WaitingPoint
    {
        public Transform Direction;
        public float TimeToWait;
    }
}

public enum EnemyLookState
{
    Pacing,
    FollowPlayer,
    Waiting
}
