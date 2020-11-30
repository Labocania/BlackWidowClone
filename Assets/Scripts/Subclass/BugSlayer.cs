using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugSlayer : Insect
{
    Transform target;
    WaitUntil waitUntilActive;
    bool isChasing = false;

    protected override void Awake()
    {
        base.Awake();
        score = 1000;
        baseSpeed = 1;

        waitUntilActive = new WaitUntil(() => target.gameObject.activeSelf == true);
        // Full Turn
        rotationAngles.Add(new Vector3(0f, 0f, 360f));
        rotationAngles.Add(new Vector3(0f, 0f, 20f));
        rotationAngles.Add(new Vector3(0f, 0f, -20f));

        waitTimes.Add(new WaitForSeconds(0.5f));
        waitTimes.Add(new WaitForSeconds(1f));
        waitTimes.Add(new WaitForSeconds(2f));
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(PickATarget());
    }

    protected override void OnDisable() => base.OnDisable();
    protected override void OnBecameInvisible()
    {
        base.OnBecameInvisible();
        StopAllCoroutines();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (isChasing)
        {
            Chase();
        }
    }

    protected override IEnumerator StartMovementRoutine()
    {
        while (gameObject.activeSelf == true && animating == false)
        {
            yield return moveComponent.TransformRotate(rotationAngles[randomNumber.Next(1, 3)], 0.5f);
        }
    }

    IEnumerator PickATarget()
    {
        while (target == null)
        {
            target = HelperMethods.SelectBug();
            yield return waitTimes[1]; // 0.5s
        }

        yield return waitUntilActive;
        Insect bug = target.GetComponent<Mosquito>();
        bug.FlashColors();

        StopCoroutine(movementRoutine);
        yield return moveComponent.TransformRotate(rotationAngles[0], 1f); // 360 turn.
        moveComponent.moveSpeed += 2;
        isChasing = true; 
    }

    protected override void Chase()
    {
        moveComponent.RotateTowards(target.position);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == target.gameObject)
        {
            moveComponent.moveSpeed = baseSpeed;
            collision.gameObject.SetActive(false);
            isChasing = false;
            movementRoutine = StartCoroutine(StartMovementRoutine());
            target = null;
            StartCoroutine(PickATarget());
        }
    }
}
