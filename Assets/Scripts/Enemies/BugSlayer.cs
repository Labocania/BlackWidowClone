﻿using System.Collections;
using UnityEngine;

public class BugSlayer : Insect
{
    Transform target;
    WaitUntil waitUntilActive;
    Coroutine pickTargetRoutine;
    bool isChasing = false;
    BugChaser bugChaser;

    protected override void Awake()
    {
        base.Awake();
        score = 1000;
        baseSpeed = 1;
        bugChaser = GetComponent<BugChaser>();
        bugChaser.moveComp = moveComponent;

        waitUntilActive = new WaitUntil(() => target.gameObject.activeSelf == true);

        EventList.noTargets += BugSlayer_OnNoTargets;
        EventList.enemyDeath += BugSlayer_OnEnemyDeath;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        bugChaser.CheckNextTarget();
    }

    protected override void OnDisable() => base.OnDisable();
    protected override void OnBecameInvisible()
    {
        bugChaser.StopChase();
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
        baseSprite.color = currentColor;
        moveComponent.MoveSpeed = baseSpeed;
        polyCollider.enabled = true;
        gameObject.layer = 8; // BugSlayer
        if (animating)
        {
            if (wasShot)
            {
                wasShot = false;
                EventList.enemyDeath.Invoke(score);
            }
            else
            {
                string name = gameObject.name.Replace("(Clone)", "");   
                EventList.enemyLeft.Invoke(name);
            }
            animating = false;
        }
    }

    protected override void onPlayerDeath()
    {
        bugChaser.StopChase();
        base.onPlayerDeath();
    }

    void BugSlayerStartExit()
    {
        RunAway(true);
    }


    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (bugChaser.IsChasing)
        {
            bugChaser.StartChase();
        }
    }

    protected override IEnumerator StartMovementRoutine()
    {
        while (gameObject.activeSelf == true && animating == false)
        {
            yield return moveComponent.TransformRotate(HelperMethods.GetRandomAngle(20f, -20f), 1f);
        }
    }


    protected override void Chase()
    {
        moveComponent.RotateTowards(target.position);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            Die();
            return;
        }

        if (collision.gameObject.CompareTag("Edge"))
        {
            moveComponent.RotateTowards(Vector2.zero);
        }

        if (bugChaser.GrubTarget != null && collision.gameObject == bugChaser.GrubTarget.gameObject)
        {
            bugChaser.StopChase();
            bugChaser.CheckNextTarget();
            movementRoutine = StartCoroutine(StartMovementRoutine());
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            bugChaser.StopChase();
        }
    }

    IEnumerator PickATarget()
    {
        while (target == null)
        {
            target = HelperMethods.SelectBug();
            yield return HelperMethods.GetWaitTime(0.5f); 
        }

        yield return waitUntilActive;
        Insect bug = target.GetComponent<Mosquito>();
        bug.FlashColors();
        yield return HelperMethods.GetWaitTime(1f);
        StopCoroutine(movementRoutine);
        moveComponent.MoveSpeed += 3;
        isChasing = true;
    }


    void BugSlayer_OnNoTargets()
    {
        bugChaser.StopChase();
        baseSprite.color = ColorList.colors[(int)ColorNames.Yellow];
        wasShot = true;
        if (this != null)
        {
            BugSlayerStartExit();
        }
    }

    void BugSlayer_OnEnemyDeath(int score)
    {
        if (gameObject.activeSelf == true && animating == false)
        {
            if (bugChaser.GrubTarget != null && bugChaser.GrubTarget.gameObject.activeSelf == false)
            {
                bugChaser.StopChase();
                bugChaser.CheckNextTarget();
                movementRoutine = StartCoroutine(StartMovementRoutine());
            }
        }
    }

    protected override void Die()
    {
        wasShot = true;
        gameObject.SetActive(false);
    }
}
