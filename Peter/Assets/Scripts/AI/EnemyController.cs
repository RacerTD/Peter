﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    protected Node AI;
    private List<EnemyAbility> enemyAbilities = new List<EnemyAbility>();
    private EnemyLook enemyLook;

    [Header("Important Declarations")]
    public Transform ViewPoint;
    public LayerMask RaycastLayerMask = new LayerMask();
    public float SightDistance = 20f;
    public float SightMaxAngle = 20f;
    public float VerticalAimOffset = 0f;

    [Header("Relevant Data")]
    public float DistanceToPlayer = 0f;
    public float ViewAngleToPlayer = 0f;
    public bool PlayerInDetectionCollider = false;
    public bool HasDirectSightLine = false;
    public float TimeSinceLastSighting = 0f;
    public Vector3 LastSeenPlayerPosition = Vector3.zero;
    public float TimeSinceShotAt = 0f;
    public bool PlayerBehindCover = false;
    public Transform CurrentCover;

    protected virtual void Start()
    {
        enemyLook = GetComponent<EnemyLook>();
        enemyAbilities = GetComponents<EnemyAbility>().ToList();
        foreach (EnemyAbility enemyAbility in enemyAbilities)
        {
            enemyAbility.Controller = this;
        }

        if (ViewPoint == null)
        {
            Debug.Break();
        }
    }

    protected virtual void Update()
    {
        DistanceToPlayer = Vector3.Distance(ViewPoint.position, GameManager.Instance.CurrentPlayer.transform.position);
        ViewAngleToPlayer = Vector3.Angle((GameManager.Instance.CurrentPlayer.transform.position + Vector3.up * VerticalAimOffset) - ViewPoint.position, ViewPoint.forward);
        HasDirectSightLine = HasDirectSight();
        TimeSinceShotAt += Time.deltaTime;

        if (CheckIfPlayerVisibleAndInRadius())
        {
            TimeSinceLastSighting = 0f;
            LastSeenPlayerPosition = GameManager.Instance.CurrentPlayer.transform.position;
        }
        else
        {
            TimeSinceLastSighting += Time.deltaTime;
        }

        if (AI != null)
        {
            AI.Evaluate();
        }
        else
        {
            Debug.LogWarning($"No intelligence found");
        }

        foreach (EnemyAbility enemyAbility in enemyAbilities.Where(e => e.TimeSinceLastUse <= e.AbilityDuration))
        {
            enemyAbility.AbilityUpdate();
        }

        foreach (EnemyAbility enemyAbility in enemyAbilities.Where(e => e.TimeSinceLastUse <= e.AbilityDuration + e.AbilityCoolDown && e.TimeSinceLastUse > e.AbilityDuration))
        {
            enemyAbility.CoolDownUpdate();
        }

        foreach (EnemyAbility enemyAbility in enemyAbilities)
        {
            enemyAbility.PermanentUpdate();
        }
    }

    /// <summary>
    /// Checks if the enemy has a direct sightline to the player
    /// </summary>
    private bool HasDirectSight()
    {
        Debug.DrawRay(ViewPoint.position, (GameManager.Instance.CurrentPlayer.transform.position + Vector3.up * VerticalAimOffset - ViewPoint.position).normalized * DistanceToPlayer, Color.green);

        RaycastHit[] hits = Physics.RaycastAll(ViewPoint.position, GameManager.Instance.CurrentPlayer.transform.position + Vector3.up * VerticalAimOffset - ViewPoint.position, RaycastLayerMask);
        hits = hits.OrderBy(h => (h.point + ViewPoint.position).magnitude).ToArray();

        for (int i = 0; i <= hits.Count() - 1; i++)
        {
            if (hits[i].collider.gameObject.layer == 15 && hits[i + 1].collider.GetComponent<Player>() != null)
            {
                CurrentCover = hits[i].transform;
                PlayerBehindCover = true;
                return false;
            }
        }

        PlayerBehindCover = false;
        CurrentCover = null;
        return hits.OrderBy(h => (h.point - ViewPoint.position).magnitude).ToArray().Select(hit => hit.collider.GetComponent<Player>() != null).FirstOrDefault();
    }

    /// <summary>
    /// Checks if the player is currently visible and in view of the enemy
    /// </summary>
    public bool CheckIfPlayerVisibleAndInRadius()
    {
        return PlayerInDetectionCollider && HasDirectSightLine;
    }

    /// <summary>
    /// Gets Calles when the enemy gets shot
    /// </summary>
    public void GotShotAt()
    {
        TimeSinceLastSighting = 0f;
        LastSeenPlayerPosition = GameManager.Instance.CurrentPlayer.transform.position;
        TimeSinceShotAt = 0f;

        if (enemyLook != null)
        {
            enemyLook.LookState = EnemyLookState.Waiting;
        }

        if (AI != null)
        {
            AI.Evaluate();
        }
    }
}
