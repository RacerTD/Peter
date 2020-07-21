using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inverter : Node
{
    //Node to evaluate
    private Node inverterNode;

    public Node InverterNode
    {
        get => inverterNode;
    }

    //Constructor
    public Inverter(Node node)
    {
        inverterNode = node;
    }

    //Inverts the state the child node returns
    public override NodeState Evaluate()
    {
        switch (inverterNode.Evaluate())
        {
            case NodeState.Failure:
                nodeState = NodeState.Success;
                return nodeState;
            case NodeState.Success:
                nodeState = NodeState.Failure;
                return nodeState;
            case NodeState.Running:
                nodeState = NodeState.Running;
                return nodeState;
        }
        nodeState = NodeState.Success;
        return nodeState;
    }
}
