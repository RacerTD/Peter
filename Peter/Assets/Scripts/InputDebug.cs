using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputDebug : MonoBehaviour
{
    public InputAction.CallbackContext currentInputAction;

    private void Awake()
    {

    }

    private void Start()
    {

    }

    private void Update()
    {
        //Debug.LogWarning("Update");
    }

    private void LateUpdate()
    {

    }

    public void Input(InputAction.CallbackContext context)
    {
        currentInputAction = context;
        //Debug.Log($"current {currentInputAction.started} {currentInputAction.performed} {currentInputAction.canceled}, context {context.started} {context.performed} {context.canceled}");
    }
}
