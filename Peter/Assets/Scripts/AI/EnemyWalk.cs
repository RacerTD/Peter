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
    private float timeToFollowPlayer = 0f;
    [SerializeField] private EnemyWalkState walkState = EnemyWalkState.Waiting;
    public EnemyWalkState WalkState
    {
        get => walkState;
        set
        {
            walkState = value;
            UpdateWalkState();
        }
    }

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        WalkState = EnemyWalkState.Waiting;
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
                navMeshAgent.destination = transform.position;
                break;
            default:
                Debug.LogWarning("");
                break;
        }
    }

    public override void PermanentUpdate()
    {
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
        if (timeToWait <= 0f)
        {
            WalkState = EnemyWalkState.WalkingToPosition;
        }
    }

    private void HandleWalkToPosition()
    {
        if (IsAtDestination())
            if (navMeshAgent.pathStatus == NavMeshPathStatus.PathComplete && navMeshAgent.pathStatus != NavMeshPathStatus.PathPartial)
            {
                WalkState = EnemyWalkState.Waiting;
            }
    }

    private void HandleFollowingPlayer()
    {
        navMeshAgent.SetDestination(GameManager.Instance.CurrentPlayer.transform.position);

        timeToFollowPlayer -= Time.deltaTime;
        if (timeToFollowPlayer <= 0f)
        {
            WalkState = EnemyWalkState.Waiting;
        }
    }

    private void HandleStopped()
    {

    }

    public void SetNewDestination(Vector3 pos)
    {
        navMeshAgent.SetDestination(pos);
    }

    public void StartFollowingPlayer(float time)
    {
        WalkState = EnemyWalkState.FollowingPlayer;
        timeToFollowPlayer = time;
    }

    public bool IsAtDestination()
    {
        return Math.Abs(CurrentPath.Positions[waitingPointsIndex].Position.position.x - transform.position.x) < 0.25f && Math.Abs(CurrentPath.Positions[waitingPointsIndex].Position.position.z - transform.position.z) < 0.25f;
    }

    [System.Serializable]
    public struct WaitingPoint
    {
        public Vector3 Destination;
        public float TimeToWait;
    }
}

public enum EnemyWalkState
{
    Waiting,
    WalkingToPosition,
    FollowingPlayer,
    Stopped
}
