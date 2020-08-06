using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorOnly : MonoBehaviour
{
    public void Start()
    {
        Destroy(gameObject);
    }
}
