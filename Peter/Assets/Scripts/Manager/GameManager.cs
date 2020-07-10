using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : ManagerModule<GameManager>
{
    public Player CurrentPlayer;

    #region GameState

    private GameState currentGameState;
    public GameState CurrentGameState
    {
        get => currentGameState;
        set
        {
            currentGameState = value;
            UpdateGameState(value);
        }
    }

    private void UpdateGameState(GameState value)
    {
        switch (value)
        {
            case GameState.Playing:
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                break;
            case GameState.Paused:
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                break;
            case GameState.CutScene:
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    #endregion

    public void Start()
    {
        CurrentGameState = GameState.Playing;
        InputSystem.pollingFrequency = 240f;
    }

    public void Update()
    {
        InputSystem.Update();

        switch (CurrentGameState)
        {
            case GameState.Playing:
                break;
            case GameState.Paused:
                break;
            case GameState.CutScene:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}

public enum GameState
{
    Playing,
    Paused,
    CutScene
}