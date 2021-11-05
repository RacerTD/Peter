using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : ManagerModule<GameManager>
{
    public Transform BulletHolder;
    public Transform ParticleHolder;
    public Transform GunHolder;
    public Player CurrentPlayer;
    private string nextSceneName = "";
    [SerializeField] private float timeTillSceneLoad = 1f;

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

    /// <summary>
    /// Happens when the gamestate gets changed
    /// </summary>
    private void UpdateGameState(GameState value)
    {
        timeInCurrentState = 0f;
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
            case GameState.Dead:
                CurrentPlayer.DisableAllAbilitys(10000f);
                UIManager.Instance.ActivateDeathScreen();
                CurrentPlayer.GetComponent<PlayerMove>().ShouldVector = Vector3.zero;
                CurrentPlayer.GetComponent<PlayerMove>().MoveVector = Vector3.zero;
                CurrentPlayer.GetComponent<PlayerShoot>().Gun.transform.SetParent(transform, true);
                CurrentPlayer.GetComponent<PlayerShoot>().Gun.WeaponAnimator.enabled = false;
                CurrentPlayer.GetComponent<PlayerShoot>().Gun.gameObject.AddComponent<Rigidbody>();
                CurrentPlayer.GetComponent<PlayerShoot>().Gun = null;
                break;
            case GameState.LoadingScene:
                CurrentPlayer.DisableAllAbilitys(10000f);
                CurrentPlayer.GetComponent<PlayerMove>().ShouldVector = Vector3.zero;
                CurrentPlayer.GetComponent<PlayerMove>().MoveVector = Vector3.zero;
                CurrentPlayer.GetComponent<PlayerShoot>().Gun.WeaponAnimator.enabled = false;
                UIManager.Instance.StartFadeToBlack(timeTillSceneLoad);
                break;
            case GameState.Quit:
                CurrentPlayer.DisableAllAbilitys(10000f);
                CurrentPlayer.GetComponent<PlayerMove>().ShouldVector = Vector3.zero;
                CurrentPlayer.GetComponent<PlayerMove>().MoveVector = Vector3.zero;
                CurrentPlayer.GetComponent<PlayerShoot>().Gun.WeaponAnimator.enabled = false;
                UIManager.Instance.StartFadeToBlack(timeTillSceneLoad);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private float timeInCurrentState = 0f;

    #endregion

    private void Start()
    {
        CurrentGameState = GameState.Playing;
    }

    private void Update()
    {
        DoGameLoop();
    }

    /// <summary>
    /// What happens during all times in the game loop
    /// </summary>
    private void DoGameLoop()
    {
        timeInCurrentState += Time.deltaTime;

        switch (CurrentGameState)
        {
            case GameState.Playing:
                break;
            case GameState.Paused:
                break;
            case GameState.CutScene:
                break;
            case GameState.Dead:
                if (timeInCurrentState >= 4)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
                break;
            case GameState.LoadingScene:
                if (timeInCurrentState >= timeTillSceneLoad)
                {
                    SceneManager.LoadScene(nextSceneName);
                }
                break;
            case GameState.Quit:
                if (timeInCurrentState >= timeTillSceneLoad)
                {
                    Application.Quit();
                    Debug.Log("Application Closed");
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    /// <summary>
    /// Switches to another scene
    /// </summary>
    /// <param name="sceneName">the name of the scene to switch to</param>
    public void SwitchToOtherScene(string sceneName)
    {
        nextSceneName = sceneName;
        CurrentGameState = GameState.LoadingScene;
    }
}

public enum GameState
{
    Playing,
    Paused,
    CutScene,
    Dead,
    LoadingScene,
    Quit
}