using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerModule<T> : MonoBehaviour where T : ManagerModule<T>
{
    protected static T _instance;

    public static T Instance
    {
        get
        {
            return _instance;
        }
    }

    public void Awake()
    {
        _instance = this as T;
    }
}
