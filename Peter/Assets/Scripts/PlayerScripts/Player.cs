using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : AbilityController
{
    private void Awake()
    {
        GameManager.Instance.CurrentPlayer = this;
    }
}
