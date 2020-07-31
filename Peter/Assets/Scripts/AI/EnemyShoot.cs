using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : EnemyAbility
{
    public List<Transform> ShootPoints = new List<Transform>();
    private int _shootPointsIndex = 0;
    private int shootPointsIndex
    {
        get => _shootPointsIndex;
        set
        {
            if (value >= ShootPoints.Count)
            {
                _shootPointsIndex = 0;
            }
            else
            {
                _shootPointsIndex = value;
            }
        }
    }
    public List<ShootStep> ShootSteps = new List<ShootStep>();
    private int _shootStepsIndex = 0;
    private int shootStepsIndex
    {
        get => _shootStepsIndex;
        set
        {
            if (value >= ShootSteps.Count)
            {
                _shootStepsIndex = 0;
            }
            else
            {
                _shootStepsIndex = value;
            }
        }
    }
    private float timeInStep = 0f;
    private float timeSinceLastShot = 0f;

    private void Start()
    {
        float temp = 0f;
        foreach (ShootStep step in ShootSteps)
        {
            temp += step.Duration;
        }
        AbilityDuration = temp;
    }

    public override void AbilityStart()
    {
        shootStepsIndex = 0;
    }

    public override void AbilityUpdate()
    {
        switch (ShootSteps[shootStepsIndex].StepType)
        {
            case StepType.Wait:
                HanldeWaitStep(ShootSteps[shootStepsIndex]);
                break;
            case StepType.Shoot:
                HandleShootStep(ShootSteps[shootStepsIndex]);
                break;
            default:
                break;
        }

        timeInStep += Time.deltaTime;
        timeSinceLastShot += Time.deltaTime;
        CheckStepEnd(ShootSteps[shootStepsIndex]);
    }

    private void HanldeWaitStep(ShootStep step)
    {

    }

    private void HandleShootStep(ShootStep step)
    {
        if (timeSinceLastShot > step.Duration / step.BulletAmount)
        {
            timeSinceLastShot = 0;

            switch (step.ShootPointMode)
            {
                case ShootPointMode.Rotate:
                    HandleShootPointModeRotate(step);
                    break;
                case ShootPointMode.AllAtOnce:
                    HandleShootPointModeAllAtOnce(step);
                    break;
                default:
                    break;
            }
        }
    }

    private void HandleShootPointModeRotate(ShootStep step)
    {
        ShootOneBullet(step.Bullet, ShootPoints[shootPointsIndex], step.BulletSpeed, step.BulletDamage, GenerateShootDirectiorn(ShootPoints[shootPointsIndex]));
        shootPointsIndex++;
    }

    private void HandleShootPointModeAllAtOnce(ShootStep step)
    {
        foreach (Transform shootPoint in ShootPoints)
        {
            ShootOneBullet(step.Bullet, shootPoint, step.BulletSpeed, step.BulletDamage, GenerateShootDirectiorn(shootPoint));
        }
    }

    private Vector3 GenerateShootDirectiorn(Transform shootPoint)
    {
        return shootPoint.forward;
    }

    private void ShootOneBullet(Bullet projectile, Transform shootPoint, float bulletVelocity, float bulletDamage, Vector3 bulletDirection)
    {
        Instantiate(projectile, shootPoint.position, shootPoint.rotation, GameManager.Instance.BulletHolder).SetupBullet(bulletVelocity, bulletDamage, bulletDirection, 10f, 1);
    }

    private void CheckStepEnd(ShootStep step)
    {
        if (timeInStep >= step.Duration)
        {
            timeInStep = 0f;
            shootStepsIndex++;
        }
    }
}

[System.Serializable]
public struct ShootStep
{
    public StepType StepType;
    public ShootPointMode ShootPointMode;
    public float Duration;
    public int BulletAmount;
    public EnemyBullet Bullet;
    public float BulletSpeed;
    public float BulletDamage;
}

public enum ShootPointMode
{
    Rotate,
    AllAtOnce
}

public enum StepType
{
    Wait,
    Shoot
}
