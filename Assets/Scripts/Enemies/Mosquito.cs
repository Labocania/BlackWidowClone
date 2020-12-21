using System.Collections;
using UnityEngine;

public class Mosquito : Insect
{
    BounceComponent bounce;

    protected override void Awake()
    {
        base.Awake();
        bounce = GetComponent<BounceComponent>();
        spawn = GetComponent<SpawnComponent>();
    }

    protected override void OnBecameInvisible()
    {
        base.OnBecameInvisible();
        HelperMethods.RemoveEdibleBug(transform);
    }

    protected override IEnumerator StartMovementRoutine()
    {
        while (gameObject.activeSelf == true && animating == false)
        {
            yield return HelperMethods.GetRandomWaitTime(1f, 2f, 5f, 8f);
            yield return moveComponent.TransformRotate(HelperMethods.GetRandomAngle(45f, -45f, 180f, -180f), 3f);
        }
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        GameObject obj = collision.gameObject;
        // Edge of the sceen or Green Web
        if (collision.enabled && gameObject.activeSelf == true)
        {
            if (movementRoutine != null)
            {
                StopCoroutine(movementRoutine);
            }

            if (bounce.BounceAmount == 0)
            {
                StartCoroutine(bounce.ResetRoutine());
                bounce.ResetBounces(8);
                movementRoutine = StartCoroutine(StartMovementRoutine());
                return;
            }

            StartCoroutine(bounce.BounceRoutine());
            bounce.DecreaseBounce();
        }
    }
}
