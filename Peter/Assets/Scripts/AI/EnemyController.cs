using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class EnemyController : MonoBehaviour
{
    private Node AI;
    private List<EnemyAbility> enemyAbilities = new List<EnemyAbility>();

    protected virtual void Start()
    {
        enemyAbilities = GetComponents<EnemyAbility>().ToList();
        AI = SetUpAI();
    }

    protected virtual void Update()
    {
        AI.Evaluate();

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

    public abstract Node SetUpAI();
}
