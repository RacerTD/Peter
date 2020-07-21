using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action : Node
{
    public override NodeState Evaluate()
    {
        switch (CheckNodeState())
        {
            case NodeState.Success:
                NodeStateSuccess();
                nodeState = NodeState.Success;
                break;
            case NodeState.Failure:
                NodeStateFailure();
                nodeState = NodeState.Failure;
                break;
            case NodeState.Running:
                NodeStateRunning();
                nodeState = NodeState.Running;
                break;
        }

        return nodeState;
    }

    public abstract NodeState CheckNodeState();

    public virtual void NodeStateSuccess()
    {

    }

    public virtual void NodeStateFailure()
    {

    }

    public virtual void NodeStateRunning()
    {

    }
}
