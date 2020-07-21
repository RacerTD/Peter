using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSelector : MonoBehaviour
{
    public Node AI;
    private void Start()
    {
        AI = new Selector(new List<Node> { new ActionOne(transform), new ActionTwo(transform) });
    }

    private void Update()
    {
        AI.Evaluate();
    }
}
