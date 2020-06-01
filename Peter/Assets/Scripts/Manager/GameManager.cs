using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : ManagerModule<GameManager>
{
    public Player CurrentPlayer;
    private GameState currentGameState;
    public GameState CurrentGameState
    {
        get { return currentGameState; }
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
                break;
            case GameState.Paused:
                break;
            case GameState.CutScene:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void Update()
    {
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