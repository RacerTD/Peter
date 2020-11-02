using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerNoClip : Player
{
    public float Speed = 10f;
    public float ViewSpeed = 10f;
    public Vector2 ViewInputVector = Vector2.zero;
    public Vector2 MovementInputVector = Vector2.zero;

    protected override void Update()
    {
        HandleView();
        HandleMovement();
    }

    public void HandleViewInput(InputAction.CallbackContext context)
    {
        if (context.started || context.performed)
        {
            ViewInputVector = context.ReadValue<Vector2>();
        }
        else
        {
            ViewInputVector = Vector2.zero;
        }
    }

    private void HandleView()
    {
        transform.rotation = Quaternion.Euler(new Vector3(-ViewInputVector.y * Time.deltaTime * ViewSpeed + transform.localRotation.eulerAngles.x, (ViewInputVector.x * Time.deltaTime * ViewSpeed) + transform.rotation.eulerAngles.y, 0));
    }

    public void HandleMovementInput(InputAction.CallbackContext context)
    {
        if (context.started || context.performed)
        {
            MovementInputVector = context.ReadValue<Vector2>();
        }
        else
        {
            MovementInputVector = Vector2.zero;
        }
    }

    public void HandleMovement()
    {
        transform.Translate(Vector3.RotateTowards(transform.position, new Vector3(MovementInputVector.x, 0f, MovementInputVector.y) * Speed * Time.deltaTime, float.MaxValue, float.MaxValue));
    }

    public void ToggleMenu()
    {
        if (GameManager.Instance.CurrentGameState == GameState.Playing)
        {
            GameManager.Instance.CurrentGameState = GameState.Paused;
        }
        else if (GameManager.Instance.CurrentGameState == GameState.Paused)
        {
            GameManager.Instance.CurrentGameState = GameState.Playing;
        }
    }

    public void LoadScene1()
    {
        GameManager.Instance.SwitchToOtherScene("1 - Entrance");
    }

    public void LoadScene2()
    {
        GameManager.Instance.SwitchToOtherScene("2 - Small Office");
    }

    public void LoadScene3()
    {
        GameManager.Instance.SwitchToOtherScene("3 - Server");
    }

    public void LoadScene4()
    {
        GameManager.Instance.SwitchToOtherScene("4 - Viewing Gallery");
    }

    public void LoadScene5()
    {
        GameManager.Instance.SwitchToOtherScene("5 - Living Quaters");
    }

    public void LoadScene6()
    {
        GameManager.Instance.SwitchToOtherScene("6 - Conservatory");
    }
}
