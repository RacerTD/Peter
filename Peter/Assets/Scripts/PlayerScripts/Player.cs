﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEditor;

public class Player : AbilityController
{
    [HideInInspector] public Camera PlayerCamera;
    [SerializeField] private List<AnimStateTime> animState = new List<AnimStateTime>();
    private PlayerShoot playerShoot;
    private bool playedAnimation = false;
    private float forcedAnimationTime = 0f;
    private WeaponAnimationState currentAnimationState = WeaponAnimationState.Idle;
    public Rumble currentRumble = new Rumble();
    public float StandardFoV = 55f;
    public float AimedFoV = 40f;
    public float FoVSpeed = 1f;

    private void Awake()
    {
        GameManager.Instance.CurrentPlayer = this;
    }

    protected override void Start()
    {
        playerShoot = GetComponent<PlayerShoot>();
        PlayerCamera = GetComponentInChildren<Camera>();
        DisableAllAbilitys(2f);
        base.Start();
    }

    protected override void Update()
    {
        if (GameManager.Instance.CurrentGameState != GameState.Dead)
        {
            HandleAnimation();

            HandleAnimationTime();
        }

        HandleRumble();
        HandleFoV();

        base.Update();
    }

    private void HandleRumble()
    {
        if (Gamepad.current != null)
        {
            if (currentRumble.Time >= 0)
            {
                Gamepad.current.SetMotorSpeeds(currentRumble.AmountL, currentRumble.AmountR);
            }
            else
            {
                Gamepad.current.SetMotorSpeeds(0, 0);
            }

            currentRumble.Time -= Time.deltaTime;
        }

#if UNITY_EDITOR
        if (!EditorApplication.isPlaying)
        {
            Gamepad.current.SetMotorSpeeds(0, 0);
        }
#endif
    }

    private void HandleFoV()
    {
        PlayerCamera.fieldOfView = Mathf.Lerp(PlayerCamera.fieldOfView, playerShoot.isAiming ? AimedFoV : StandardFoV, FoVSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Handles all the animations of the gun
    /// </summary>
    private void HandleAnimation()
    {
        if (playedAnimation)
        {
            forcedAnimationTime -= Time.deltaTime;

            if (forcedAnimationTime <= 0f)
            {
                playedAnimation = false;
            }

            return;
        }

        if (animState.Count <= 0)
        {
            AddAnimState(WeaponAnimationState.Idle, 0f);
        }

        animState = animState.OrderBy(a => a.Priority).ToList();

        if (currentAnimationState != animState[0].State)
        {
            //Debug.Log(animState[0].State);

            switch (animState[0].State)
            {
                case WeaponAnimationState.Idle:
                    playerShoot.Gun.WeaponAnimator.SetTrigger("Idle");
                    break;
                case WeaponAnimationState.Idle2:
                    playerShoot.Gun.WeaponAnimator.SetTrigger("Idle2");
                    break;
                case WeaponAnimationState.Walking:
                    playerShoot.Gun.WeaponAnimator.SetTrigger(playerShoot.isAiming ? "Idle" : "Walking");
                    break;
                case WeaponAnimationState.Running:
                    playerShoot.Gun.WeaponAnimator.SetTrigger("Running");
                    break;
                case WeaponAnimationState.Reload:
                    playerShoot.Gun.WeaponAnimator.SetTrigger("Reload");
                    break;
                case WeaponAnimationState.Recoil:
                    playerShoot.Gun.WeaponAnimator.SetTrigger("Idle");
                    playerShoot.Gun.WeaponAnimator.SetTrigger("Recoil");
                    break;
                case WeaponAnimationState.RecoilScope:
                    playerShoot.Gun.WeaponAnimator.SetTrigger("Idle");
                    playerShoot.Gun.WeaponAnimator.SetTrigger("RecoilScope");
                    break;
                default:
                    playerShoot.Gun.WeaponAnimator.SetTrigger("Idle");
                    break;
            }

            currentAnimationState = animState[0].State;
        }
    }

    /// <summary>
    /// Calculates all the timings for the animations
    /// </summary>
    private void HandleAnimationTime()
    {
        for (int i = 0; i < animState.Count(); i++)
        {
            animState[i].RemainingTime -= Time.deltaTime;
        }

        List<AnimStateTime> animTemp = new List<AnimStateTime>();

        foreach (AnimStateTime thing in animState)
        {
            if (thing.RemainingTime <= 0f)
            {
                animTemp.Add(thing);
            }
        }

        foreach (AnimStateTime anim in animTemp)
        {
            animState.Remove(anim);
        }
    }

    /// <summary>
    /// Adds a new animation state to the animation que
    /// </summary>
    /// <param name="state">the state the gun should take</param>
    /// <param name="time">the time the state should be held for</param>
    public void AddAnimState(WeaponAnimationState state, float time)
    {
        int prio = 100;
        switch (state)
        {
            case WeaponAnimationState.Idle:
                prio = 10;
                break;
            case WeaponAnimationState.Idle2:
                prio = 9;
                break;
            case WeaponAnimationState.Walking:
                prio = 8;
                break;
            case WeaponAnimationState.Running:
                prio = 7;
                break;
            case WeaponAnimationState.Reload:
                prio = 1;
                break;
            case WeaponAnimationState.Recoil:
                prio = 2;
                break;
            case WeaponAnimationState.RecoilScope:
                prio = 2;
                break;
            default:
                prio = 100;
                break;
        }

        animState.Add(new AnimStateTime(state, time, prio));
    }

    /// <summary>
    /// Plays an animation right now
    /// </summary>
    /// <param name="animationState">The animation to play</param>
    /// <param name="time">How long the animationstate should last</param>
    public void PlayAnimationNow(WeaponAnimationState animationState, float time)
    {
        playedAnimation = true;
        forcedAnimationTime = time;

        playerShoot.Gun.WeaponAnimator.SetTrigger("Idle");

        switch (animationState)
        {
            case WeaponAnimationState.Idle:
                playerShoot.Gun.WeaponAnimator.SetTrigger("Idle");
                break;
            case WeaponAnimationState.Idle2:
                playerShoot.Gun.WeaponAnimator.SetTrigger("Idle2");
                break;
            case WeaponAnimationState.Walking:
                playerShoot.Gun.WeaponAnimator.SetTrigger("Walking");
                break;
            case WeaponAnimationState.Running:
                playerShoot.Gun.WeaponAnimator.SetTrigger("Running");
                break;
            case WeaponAnimationState.Reload:
                playerShoot.Gun.WeaponAnimator.SetTrigger("Reload");
                break;
            case WeaponAnimationState.Recoil:
                playerShoot.Gun.WeaponAnimator.SetTrigger("Recoil");
                break;
            case WeaponAnimationState.RecoilScope:
                playerShoot.Gun.WeaponAnimator.SetTrigger("RecoilScope");
                break;
            default:
                playerShoot.Gun.WeaponAnimator.SetTrigger("Idle");
                break;
        }
    }

    [System.Serializable]
    private class AnimStateTime
    {
        public WeaponAnimationState State;
        public float RemainingTime;
        public int Priority;

        public AnimStateTime(WeaponAnimationState state, float time, int priority)
        {
            State = state;
            RemainingTime = time;
            Priority = priority;
        }
    }
}

[System.Serializable]
public struct Rumble
{
    public float AmountL;
    public float AmountR;
    public float Time;

    public Rumble(float amountL, float amountR, float time)
    {
        AmountL = amountL;
        AmountR = amountR;
        Time = time;
    }
}

public enum WeaponAnimationState
{
    Idle,
    Idle2,
    Walking,
    Running,
    Reload,
    Recoil,
    RecoilScope
}