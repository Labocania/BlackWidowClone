using System.Collections;
using UnityEngine;

public class Mosquito : Insect
{
    public GameObject grub;
    int bounceAmount;

    protected override void Awake()
    {
        base.Awake();
        
        //Left rotation
        rotationAngles.Add(new Vector3(0f, 0f, 45f));
        //Right rotation
        rotationAngles.Add(new Vector3(0f, 0f, -45f));
        //U-turn left
        rotationAngles.Add(new Vector3(0f, 0f, 180f));
        //U-turn right
        rotationAngles.Add(new Vector3(0f, 0f, -180f));

        waitTimes.Add(new WaitForSeconds(1f));
        waitTimes.Add(new WaitForSeconds(2f));
        waitTimes.Add(new WaitForSeconds(5f));
        waitTimes.Add(new WaitForSeconds(8f));

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
            yield return waitTimes[randomNumber.Next(1, waitTimes.Count)];
            yield return moveComponent.TransformRotate(rotationAngles[randomNumber.Next(rotationAngles.Count)], 3f);
        }
    }

    IEnumerator WaitOneSecond()
    {
        yield return waitTimes[1];
    }

    IEnumerator BounceRoutine()
    {
        //Snap rotate 180 degrees.
        gameObject.transform.Rotate(rotationAngles[2]);
        //Pick left or right U turn
        yield return moveComponent.TransformRotate(rotationAngles[randomNumber.Next(2, 4)], 0.5f);
    }

    IEnumerator ResetRoutine()
    {
        //Snap rotate 180 degrees.
        gameObject.transform.Rotate(rotationAngles[2]);
        yield return WaitOneSecond();

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
            //StartCoroutine(WaitOneSecond());
            //movementRoutine = StartCoroutine(StartMovementRoutine());
        }
    }

    protected override void Spawn()
    {
        Instantiate(grub, transform.position, Quaternion.identity);
    }

}
