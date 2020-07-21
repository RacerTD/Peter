using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionOne : Action
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
            return NodeState.Success;
        }

        return NodeState.Failure;
    }

    public override void NodeStateSuccess()
    {
        bla.position += Vector3.left * Time.deltaTime;
    }
}
