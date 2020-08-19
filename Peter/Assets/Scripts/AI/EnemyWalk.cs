using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyWalk : EnemyAbility
{
    private NavMeshAgent navMeshAgent;
    public EnemyPath CurrentPath;
    private int _waitingPointsIndex = 0;
    private int waitingPointsIndex
    {
        get => _waitingPointsIndex;
        set
        {
            if (value >= CurrentPath.Positions.Count)
            {
                _waitingPointsIndex = 0;
            }
            else
            {
                _waitingPointsIndex = value;
            }
        }
    }
    private float timeToWait = 0f;
    public Vector3 currentDestination = Vector3.zero;
    public Vector3 CurrentDestination
    {
        get => currentDestination;
        set
        {
            if (value != currentDestination)
            {
                currentDestination = value;
                navMeshAgent.destination = value;
            }
        }
    }

    [SerializeField] private EnemyWalkState walkState = EnemyWalkState.Waiting;
    public EnemyWalkState WalkState
    {
        get => walkState;
        set
        {
            if (value != walkState)
            {
                timeInCurrentState = 0f;
            }

            walkState = value;
            UpdateWalkState();
        }
    }
    private float timeInCurrentState = 0f;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void UpdateWalkState()
    {
        switch (WalkState)
        {
            case EnemyWalkState.Waiting:
                break;
            case EnemyWalkState.WalkingToPosition:
                if (CurrentPath != null)
                {
                    waitingPointsIndex++;

                    navMeshAgent.SetDestination(CurrentPath.Positions[waitingPointsIndex].Position.position);
                    timeToWait = CurrentPath.Positions[waitingPointsIndex].WaitTimeAtPostion;
                }
                break;
            case EnemyWalkState.FollowingPlayer:
                break;
            case EnemyWalkState.Stopped:
                CurrentDestination = transform.position;
                break;
            default:
                Debug.LogWarning("");
                break;
        }
    }

    private void DecideNewWalkState()
    {
        if (Controller.CheckIfPlayerVisibleAndInRadiusAndNotBehindCover() && Controller.DistanceToPlayer >= 10f)
        {
            WalkState = EnemyWalkState.FollowingPlayer;
        }
        else if (Controller.CheckIfPlayerVisibleAndInRadiusAndNotBehindCover() && Controller.DistanceToPlayer <= 10f)
        {
            WalkState = EnemyWalkState.Stopped;
        }
        else if (Controller.TimeSinceLastSighting <= 10f)
        {
            WalkState = EnemyWalkState.FollowingPlayer;
        }
        else if (WalkState == EnemyWalkState.WalkingToPosition && IsAtDestination())
        {
            WalkState = EnemyWalkState.Waiting;
        }
        else if (WalkState == EnemyWalkState.Waiting && timeToWait < 0f)
        {
            WalkState = EnemyWalkState.WalkingToPosition;
        }
        else if (timeInCurrentState >= 20)
        {
            WalkState = EnemyWalkState.WalkingToPosition;
        }
    }

    private void WalkStateOverride()
    {
        if (Controller.CheckIfPlayerVisibleAndInRadiusAndNotBehindCover() && Controller.DistanceToPlayer >= 10f)
        {
            WalkState = EnemyWalkState.FollowingPlayer;
        }
        else if (Controller.CheckIfPlayerVisibleAndInRadiusAndNotBehindCover() && Controller.DistanceToPlayer <= 10f)
        {
            WalkState = EnemyWalkState.Stopped;
        }
        else if (Controller.TimeSinceLastSighting <= 10f)
        {
            WalkState = EnemyWalkState.FollowingPlayer;
        }
    }

    public override void PermanentUpdate()
    {
        timeInCurrentState += Time.deltaTime;

        WalkStateOverride();

        switch (WalkState)
        {
            case EnemyWalkState.Waiting:
                HanldeWaiting();
                break;
            case EnemyWalkState.WalkingToPosition:
                HandleWalkToPosition();
                break;
            case EnemyWalkState.FollowingPlayer:
                HandleFollowingPlayer();
                break;
            case EnemyWalkState.Stopped:
                HandleStopped();
                break;
            default:
                Debug.LogWarning("");
                break;
        }
        base.PermanentUpdate();
    }

    private void HanldeWaiting()
    {
        timeToWait -= Time.deltaTime;

        if (timeToWait <= 0)
        {
            DecideNewWalkState();
        }
    }

    private void HandleWalkToPosition()
    {
        if (IsAtDestination())
        {
            DecideNewWalkState();
        }
    }

    private void HandleFollowingPlayer()
    {
        CurrentDestination = Controller.LastSeenPlayerPosition;

        if (timeInCurrentState >= 10f && !Controller.CheckIfPlayerVisibleAndInRadiusAndNotBehindCover())
        {
            DecideNewWalkState();
        }
    }

    private void HandleStopped()
    {
        if (timeInCurrentState >= 10f)
        {
            DecideNewWalkState();
        }
    }

    public void SetNewDestination(Vector3 pos)
    {
        CurrentDestination = pos;
    }

    public bool IsAtDestination()
    {
        //Debug.Log($"{Mathf.Abs(navMeshAgent.destination.x - transform.position.x)}, {Mathf.Abs(navMeshAgent.destination.z - transform.position.z)}");
        return Mathf.Abs(navMeshAgent.destination.x - transform.position.x) < 1 && Mathf.Abs(navMeshAgent.destination.z - transform.position.z) < 1;
    }
}

public enum EnemyWalkState
{
    Waiting,
    WalkingToPosition,
    FollowingPlayer,
    Stopped
}
