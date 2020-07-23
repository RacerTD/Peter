using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base Node, nothing more to say
/// </summary>
public abstract class Node
{
    //return current state of the node
    public delegate NodeState NodeReturn();
    //current node state
    protected NodeState nodeState;
    public NodeState NodeState
    {
        get => nodeState;
    }

    //Constructor
    public Node() { }

    //Evaluation funktion what the node is doing
    public abstract NodeState Evaluate();
}

public enum NodeState
{
    Failure,
    Success,
    Running
}