using System.Collections;
using UnityEngine;

public class Mosquito : Insect
{
    public GameObject grub;
    BounceComponent bounce;

    protected override void Awake()
    {
        base.Awake();
        bounce = GetComponent<BounceComponent>();
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

    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject obj = collision.gameObject;

        if (obj.CompareTag("Projectile"))
        {
            Spawn();
            base.Die();
            return;
        }

        if (obj.CompareTag("BugSlayer") && flashing == true)
        {
            base.Die();
            flashing = false;
            return;
        }

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
                //Reset amount with another random number
                bounce.ResetBounces(8);
                movementRoutine = StartCoroutine(StartMovementRoutine());
                return;
            }

            StartCoroutine(bounce.BounceRoutine());
            bounce.DecreaseBounce();
        }
    }

    protected override void Spawn()
    {
        Instantiate(grub, transform.position, Quaternion.identity);
    }
}
