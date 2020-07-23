using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TurretView))]
[RequireComponent(typeof(TurretShoot))]
public class TurretController : EnemyController
{
    public override Node SetUpAI()
    {

        AIAimingNode aimNode = new AIAimingNode(GetComponent<TurretView>(), GameManager.Instance.CurrentPlayer);
        AIShootingNode shootNode = new AIShootingNode(GetComponent<TurretShoot>(), GetComponent<TurretView>(), GameManager.Instance.CurrentPlayer, 10f);

        Sequence aimShootSequence = new Sequence(new List<Node> { aimNode, shootNode });

        AIViewPacingNode pacingNode = new AIViewPacingNode(GetComponent<TurretView>());

        return new Selector(new List<Node> { aimShootSequence, pacingNode });
    }
}
