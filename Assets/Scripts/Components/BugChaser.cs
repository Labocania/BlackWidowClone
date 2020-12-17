using System.Collections;

public class BugChaser : ChasingType
{
    bool speedUp = false;

    protected override void Awake()
    {
        base.Awake();
        PickATargetRoutine = StartCoroutine(PickTarget());
    }

    protected override IEnumerator PickTarget()
    {
        yield return base.PickTarget();
        IsChasing = true;
    }

    void SpeedUp()
    {
        moveComponent.MoveSpeed += 3;
        speedUp = true;
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

    public override void StopAllChases()
    {
        base.StopAllChases();
        if (speedUp)
        {
            moveComponent.MoveSpeed -= 3;
            speedUp = false;
        }
    }
}
