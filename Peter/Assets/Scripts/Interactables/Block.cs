using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[ExecuteInEditMode]
public class Block : MonoBehaviour
{
    [SerializeField] private float timeToOpen = 0f;
    [SerializeField] private GameObject door;
    [SerializeField] public int height;
    
    [SerializeField] private BlockState blockState = BlockState.Close;
    private bool openClose;
    private Vector3 originalPosition;
    public float timeToGoBack;
    float t;

    private enum BlockState
    {
        Open,
        Close,
        Locked
    }

    private void Start()
    {
        originalPosition = door.transform.position;
    }
    public void Update()
    {
        switch (blockState)
        {
            case BlockState.Open:
                if(openClose == false)
                {
                    t = 0;
                    door.transform.position = new Vector3(door.transform.position.x, door.transform.position.y + height, door.transform.position.z);                
                    openClose = true;
                }
                t += Time.deltaTime / timeToGoBack;
                door.transform.position = Vector3.Lerp(door.transform.position, originalPosition, t);
                if(door.transform.position.normalized == originalPosition.normalized)
                    blockState = BlockState.Close;
                break;
            case BlockState.Close:
                if (openClose == true)
                {
                    door.transform.position = originalPosition;
                    openClose = false;
                }
                break;
            case BlockState.Locked:
                break;
            default:
                Debug.LogError("I have a bad feeling about this.");
                break;
        }
    }
    public void Open()
    {
        if (openClose == false)
        {
            blockState = BlockState.Open;
            openClose = true;
        }
        else
            return;
    }
    private void Close()
    {
        if (openClose == true)
        {
            blockState = BlockState.Close;
            openClose = false;
        }
        else
            return;
    }

    public void OpenCloseBlock()
    {
        switch (blockState)
        {
            case BlockState.Open:
                blockState = BlockState.Close;
                break;
            case BlockState.Close:
                blockState = BlockState.Open;
                break;
            case BlockState.Locked:
                break;
        }
    }

    public void OpenCloseCall(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Open();
            Close();
            OpenCloseBlock();
        }

    }

}
