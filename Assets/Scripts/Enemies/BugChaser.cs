﻿using System.Collections;
using UnityEngine;

public class BugChaser : ChasingType
{
    public Coroutine PickATargetRoutine { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        PickATargetRoutine = StartCoroutine(PickTarget());
    }

    IEnumerator PickTarget()
    {
        while (GrubTarget == null)
        {
            GrubTarget = HelperMethods.SelectBug();
            yield return HelperMethods.GetWaitTime(0.5f);
        }

        IsChasing = true;
    }

    void SpeedUp()
    {
        moveComp.MoveSpeed += 3;
    }

    protected override IEnumerator ChaseRoutine()
    {
        yield return waitUntilActive;
        if (!GrubTarget.gameObject.CompareTag("Grub"))
        {
            FlashTarget();
        }
        yield return HelperMethods.GetWaitTime(1.5f);
        SpeedUp();
        yield return base.ChaseRoutine();
    }

    void FlashTarget()
    {
        Insect bug = GrubTarget.GetComponent<Insect>();
        bug.FlashColors();
    }

    public override void StopChase()
    {
        if (PickATargetRoutine != null)
        {
            StopCoroutine(PickATargetRoutine);
        }
        base.StopChase();
        moveComp.MoveSpeed -= 3;    
    }

    public void CheckNextTarget()
    {
        PickATargetRoutine = StartCoroutine(PickTarget());
    }
}
