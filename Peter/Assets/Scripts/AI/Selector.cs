using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Executes the first node that works
/// </summary>
public class Selector : Node
{
    //List of nodes to select from
    protected List<Node> nodes = new List<Node>();

    //Constructor
    public Selector(List<Node> nodes)
    {
        this.nodes = nodes;
    }

    //Evalueates all child nodes
    public override NodeState Evaluate()
    {
        //Debug.Log(nodes.Count);

        foreach (Node node in nodes)
        {
            //Debug.Log("bla");

            switch (node.Evaluate())
            {
                case NodeState.Failure:
                    continue;
                case NodeState.Success:
                    nodeState = NodeState.Success;
                    return nodeState;
                case NodeState.Running:
                    nodeState = NodeState.Running;
                    return nodeState;
                default:
                    continue;
            }
        }

        nodeState = NodeState.Failure;
        return nodeState;
    }
}
