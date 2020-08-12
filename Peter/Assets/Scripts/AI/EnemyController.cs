using System.Collections;
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

    [Header("Relevant Data")]
    public float DistanceToPlayer = 0f;
    public float ViewAngleToPlayer = 0f;
    public bool PlayerInDetectionCollider = false;
    public bool HasDirectSightLine = false;
    public float TimeSinceLastSighting = 0f;
    public Vector3 LastSeenPlayerPosition = Vector3.zero;
    public float TimeSinceShotAt = 0f;
    public bool PlayerBehindCover = false;
    public float TimeBehindCover = 0f;
    public Transform CurrentCover;
    public Vector3 CurrentDirectionToPlayer = Vector3.zero;

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
        UpdateData();

        if (AI != null)
        {
            AI.Evaluate();
        }
        else
        {
            Debug.LogWarning($"No intelligence found");
        }

        foreach (EnemyAbility enemyAbility in enemyAbilities.Where(e => e.TimeSinceLastUse <= e.AbilityDuration))
            enemyAbility.AbilityUpdate();

        foreach (EnemyAbility enemyAbility in enemyAbilities.Where(e => e.TimeSinceLastUse <= e.AbilityDuration + e.AbilityCoolDown && e.TimeSinceLastUse > e.AbilityDuration))
            enemyAbility.CoolDownUpdate();

        foreach (EnemyAbility enemyAbility in enemyAbilities)
            enemyAbility.PermanentUpdate();
    }

    /// <summary>
    /// Updates all the important data for the enemy
    /// </summary>
    private void UpdateData()
    {
        DistanceToPlayer = Vector3.Distance(ViewPoint.position, GameManager.Instance.CurrentPlayer.PlayerCamera.transform.position);
        ViewAngleToPlayer = Vector3.Angle((GameManager.Instance.CurrentPlayer.PlayerCamera.transform.position) - ViewPoint.position, ViewPoint.forward);
        HasDirectSightLine = HasDirectSight();
        TimeSinceShotAt += Time.deltaTime;
        CurrentDirectionToPlayer = (GameManager.Instance.CurrentPlayer.PlayerCamera.transform.position - transform.position).normalized;

        if (CheckIfPlayerVisibleAndInRadiusAndNotBehindCover())
        {
            TimeSinceLastSighting = 0f;
            LastSeenPlayerPosition = GameManager.Instance.CurrentPlayer.PlayerCamera.transform.position;
        }
        else
        {
            TimeSinceLastSighting += Time.deltaTime;
        }

        if (PlayerBehindCover)
        {
            TimeBehindCover += Time.deltaTime;
        }
        else
        {
            TimeBehindCover = 0f;
        }
    }

    /// <summary>
    /// Checks if the enemy has a direct sightline to the player
    /// </summary>
    private bool HasDirectSight()
    {
        //Debug.DrawRay(ViewPoint.position, (GameManager.Instance.CurrentPlayer.transform.position + Vector3.up * VerticalAimOffset - ViewPoint.position).normalized * DistanceToPlayer, Color.green);

        RaycastHit[] hits = Physics.RaycastAll(ViewPoint.position, GameManager.Instance.CurrentPlayer.PlayerCamera.transform.position - ViewPoint.position, RaycastLayerMask);
        hits = hits.OrderBy(h => (h.point + ViewPoint.position).magnitude).ToArray();

        if (hits.Count() >= 2)
        {
            for (int i = 0; i <= hits.Count() - 1; i++)
            {
                if (hits[i].collider.gameObject.layer == 15 && hits[i + 1].collider.GetComponent<Player>() != null)
                {
                    CurrentCover = hits[i].transform;
                    PlayerBehindCover = true;
                    return false;
                }
            }
        }

        PlayerBehindCover = false;
        CurrentCover = null;
        return hits.OrderBy(h => (h.point - ViewPoint.position).magnitude).ToArray().Select(hit => hit.collider.GetComponent<Player>() != null).FirstOrDefault();
    }

    /// <summary>
    /// Checks if the player is currently visible and in view of the enemy
    /// </summary>
    public bool CheckIfPlayerVisibleAndInRadiusAndNotBehindCover()
    {
        return PlayerInDetectionCollider && HasDirectSightLine && !PlayerBehindCover;
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
