using System.Collections;
using UnityEngine;

public abstract class ChasingType : MonoBehaviour
{
    public Transform GrubTarget { get; protected set; }
    public Transform PlayerTarget { get; set; }
    [System.NonSerialized]
    public MovementComponent moveComp;
    public bool IsChasing { get; protected set; }
    protected Coroutine chaseRoutine;
    protected WaitUntil waitUntilActive;

    protected virtual void Awake()
    {
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
    void ChaseTarget()
    {
        if (PlayerTarget != null)
        {
            moveComp.RotateTowards(PlayerTarget.position);
        }
        else
        {
            moveComp.RotateTowards(GrubTarget.position);
        }
    }

    public void StartChase()
    {
        chaseRoutine = StartCoroutine(ChaseRoutine());
        IsChasing = false;
    }

    public virtual void StopChase()
    {
        if (chaseRoutine != null)
        {
            StopCoroutine(chaseRoutine);
        }
        GrubTarget = null;
        PlayerTarget = null;
    }
}
