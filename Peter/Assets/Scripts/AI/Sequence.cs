using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequence : Node
{
    //Nodes that belong to this sequence
    protected List<Node> nodes = new List<Node>();

    //Constructor
    public Sequence(List<Node> nodes)
    {
        this.nodes = nodes;
    }

    public override NodeState Evaluate()
    {
        bool anyChildRunning = false;

        foreach (Node node in nodes)
        {
            switch (node.Evaluate())
            {
                case NodeState.Failure:
                    nodeState = NodeState.Failure;
                    return nodeState;
                case NodeState.Success:
                    continue;
                case NodeState.Running:
                    anyChildRunning = true;
                    continue;
                default:
                    nodeState = NodeState.Success;
                    return nodeState;
            }
        }

        nodeState = anyChildRunning ? NodeState.Running : NodeState.Success;
        return nodeState;
    }
}
