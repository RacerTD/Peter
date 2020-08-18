using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : AbilityController
{
    [HideInInspector] public Camera PlayerCamera;
    [SerializeField] private List<AnimStateTime> animState = new List<AnimStateTime>();
    private PlayerShoot playerShoot;
    private bool playedAnimation = false;
    private float forcedAnimationTime = 0f;
    private WeaponAnimationState currentAnimationState = WeaponAnimationState.Idle;

    private void Awake()
    {
        GameManager.Instance.CurrentPlayer = this;
    }

    protected override void Start()
    {
        playerShoot = GetComponent<PlayerShoot>();
        PlayerCamera = GetComponentInChildren<Camera>();
        DisableAllAbilitys(3f);
        base.Start();
    }

    protected override void Update()
    {
        if (GameManager.Instance.CurrentGameState != GameState.Dead)
        {
            HandleAnimation();

            HandleAnimationTime();
        }
        base.Update();
    }

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

    public void AddAnimState(WeaponAnimationState state, float time)
    {
        int prio = 0;
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