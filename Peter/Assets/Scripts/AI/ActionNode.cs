using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionNode : Node
{
    public override NodeState Evaluate()
    {
        switch (CheckNodeState())
        {
            case NodeState.Success:
                NodeStateSuccess();
                nodeState = NodeState.Success;
                return nodeState;
            case NodeState.Failure:
                NodeStateFailure();
                nodeState = NodeState.Failure;
                return nodeState;
            case NodeState.Running:
                NodeStateRunning();
                nodeState = NodeState.Running;
                return nodeState;
            default:
                nodeState = NodeState.Failure;
                return nodeState;

        }
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
