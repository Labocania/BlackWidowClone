using System.Collections;
using UnityEngine;

public class Mosquito : Insect
{
    public GameObject grub;
    int bounceAmount;

    protected override void Awake()
    {
        base.Awake();
        bounceAmount = randomNumber.Next(8);
    }

    protected override void OnEnable() => base.OnEnable();

    protected override void OnDisable() => base.OnDisable();

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

    IEnumerator BounceRoutine()
    {
        //Snap rotate 180 degrees.
        gameObject.transform.Rotate(HelperMethods.GetRotationAngle(180f));
        //Pick left or right U turn
        yield return moveComponent.TransformRotate(HelperMethods.GetRandomAngle(180f, -180f), 0.5f);
    }

    IEnumerator ResetRoutine()
    {
        //Snap rotate 180 degrees.
        gameObject.transform.Rotate(HelperMethods.GetRotationAngle(180f));
        yield return HelperMethods.GetWaitTime(1f);

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

            if (bounceAmount == 0)
            {
                StartCoroutine(ResetRoutine());
                //Reset amount with another random number
                bounceAmount = randomNumber.Next(8);
                movementRoutine = StartCoroutine(StartMovementRoutine());
                return;
            }

            StartCoroutine(BounceRoutine());
            bounceAmount--;
        }
    }

    protected override void Spawn()
    {
        Instantiate(grub, transform.position, Quaternion.identity);
    }
}
