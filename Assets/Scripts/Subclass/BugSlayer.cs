using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugSlayer : Insect
{
    protected override void Awake()
    {
        base.Awake();

        // Full Turn
        rotationAngles.Add(new Vector3(0f, 0f, 360f));
        rotationAngles.Add(new Vector3(0f, 0f, 20f));
        rotationAngles.Add(new Vector3(0f, 0f, -20f));

        waitTimes.Add(new WaitForSeconds(1f));
        waitTimes.Add(new WaitForSeconds(0.5f));
    }

    void FixedUpdate()
    {
        if (gameObject.activeSelf == true)
        {
            moveComponent.TransformMove(transform.up);
        }
    }

    protected override IEnumerator StartMovementRoutine()
    {
        while (gameObject.activeSelf == true && animating == false)
        {
            yield return moveComponent.TransformRotate(rotationAngles[randomNumber.Next(1, 3)], 0.4f);
        }
    }

    protected override void Chase()
    {
        base.Chase();
    }
}
