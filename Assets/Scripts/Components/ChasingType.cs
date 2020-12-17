using System.Collections;
using UnityEngine;

public abstract class ChasingType : MonoBehaviour
{
    public Transform GrubTarget { get; protected set; }
    public Transform PlayerTarget { get; set; }
    [System.NonSerialized]
    protected MovementComponent moveComponent;
    public bool IsChasing { get; protected set; }
    protected Coroutine chaseRoutine;
    public Coroutine PickATargetRoutine { get; protected set; }
    protected WaitUntil waitUntilActive;

    protected virtual void Awake()
    {
        moveComponent = GetComponent<MovementComponent>();
        waitUntilActive = new WaitUntil(() => GrubTarget.gameObject.activeSelf == true);
        IsChasing = false;
    }

    protected virtual IEnumerator ChaseRoutine()
    {
        while (GrubTarget != null || PlayerTarget != null)
        {
            ChaseTarget();
            yield return null;
        }
    }

    protected virtual IEnumerator PickTarget()
    {
        while (GrubTarget == null)
        {
            GrubTarget = HelperMethods.SelectBug();
            yield return HelperMethods.GetWaitTime(0.5f);
        }
    }

    void ChaseTarget()
    {
        if (GrubTarget != null)
        {
            moveComponent.RotateTowards(GrubTarget.position);
        }
        else
        {
            moveComponent.RotateTowards(PlayerTarget.position);
        }
    }

    public void StartChase()
    {
        chaseRoutine = StartCoroutine(ChaseRoutine());
        IsChasing = false;
    }

    public virtual void StopAllChases()
    {
        StopBugChase();

        if (chaseRoutine != null)
        {
            StopCoroutine(chaseRoutine);
        }

        PlayerTarget = null;
        IsChasing = false;
    }

    public virtual void StopBugChase()
    {
        GrubTarget = null;
        if (PickATargetRoutine != null)
        {
            StopCoroutine(PickATargetRoutine);
        }
    }


    public void CheckNextTarget()
    {
        PickATargetRoutine = StartCoroutine(PickTarget());
    }

    public void SetPlayerTarget()
    {
        PlayerTarget = HelperMethods.playerTransform;
        IsChasing = true;
    }
}
