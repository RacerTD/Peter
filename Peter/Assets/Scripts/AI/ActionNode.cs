using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionNode : Node
{
    //Method signateure for the action
    public delegate NodeState ActionNodeDelegate();
    //The delegate that is called for evaluation
    private Action action;

    //Constructor
    public ActionNode(Action action)
    {
        this.action = action;
    }

    public override NodeState Evaluate()
    {
        switch (action.Evaluate())
        {
            case NodeState.Success:
                nodeState = NodeState.Success;
                return nodeState;
            case NodeState.Failure:
                nodeState = NodeState.Failure;
                return nodeState;
            case NodeState.Running:
                nodeState = NodeState.Running;
                return nodeState;
            default:
                nodeState = NodeState.Failure;
                return nodeState;
        }
    }
}
