using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MichelleBotController : EnemyController
{
    protected override void Update()
    {
        AI.Evaluate();
        base.Update();
    }

    private void OnDrawGizmos()
    {
        if (ViewPoint != null)
        {
            Debug.DrawRay(ViewPoint.position, ViewPoint.forward, Color.red);
        }
    }
}
