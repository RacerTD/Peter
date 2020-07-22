using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionOne : ActionNode
{
    private Transform bla;

    public ActionOne(Transform bla)
    {
        this.bla = bla;
    }

    public override NodeState CheckNodeState()
    {
        if (bla.position.y > 0)
        {
            return NodeState.Running;
        }

        return NodeState.Failure;
    }

    public override void NodeStateRunning()
    {
        bla.position += Vector3.left * Time.deltaTime;
    }
}
