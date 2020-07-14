using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : AbilityController
{
    protected override void Start()
    {
        GameManager.Instance.CurrentPlayer = this;
        base.Start();
    }
}
