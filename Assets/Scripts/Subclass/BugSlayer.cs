using System.Collections;
using UnityEngine;

public class BugSlayer : Insect
{
    Transform target;
    WaitUntil waitUntilActive;
    Coroutine pickTargetRoutine;
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
        EventList.waveChanged += BugSlayer_OnWaveChanged;
        EventList.noTargets += BugSlayer_OnNoTargets;
        EventList.enemyDeath += BugSlayer_OnEnemyDeath;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        pickTargetRoutine = StartCoroutine(PickATarget());
    }

    protected override void OnDisable() => base.OnDisable();
    protected override void OnBecameInvisible()
    {
        target = null;
        gameObject.SetActive(false);
        baseSprite.color = currentColor;
        moveComponent.moveSpeed = baseSpeed;
        polyCollider.enabled = true;
        if (animating)
        {
            if (wasShot)
            {
                wasShot = false;
                EventList.enemyDeath.Invoke(score);
            }
            else
            {
                string name = gameObject.name.Replace("(Clone)", "");   
                EventList.enemyLeft.Invoke(name);
            }
            animating = false;
        }
    }

    protected override void onPlayerDeath()
    {
        isChasing = false;
        StopCoroutine(pickTargetRoutine);
        base.onPlayerDeath();
    }

    void BugSlayer_OnWaveChanged()
    {
        //BugSlayerStartExit();
        //RunAway();
    }

    void BugSlayerStartExit()
    {
        wasShot = true;
        RunAway();
    }


    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (isChasing && !animating)
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


    protected override void Chase()
    {
        moveComponent.RotateTowards(target.position);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (target != null && collision.gameObject == target.gameObject)
        {
            CheckNextTarget();
            return;
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            isChasing = false;
            StopCoroutine(pickTargetRoutine);
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
        polyCollider.enabled = false; // Fix this.
        yield return moveComponent.TransformRotate(rotationAngles[0], 1f); // 360 turn.
        polyCollider.enabled = true;
        moveComponent.moveSpeed += 3;
        isChasing = true;
    }


    void BugSlayer_OnNoTargets()
    {
        isChasing = false;
        target = null;
        moveComponent.moveSpeed = baseSpeed;
        if (this != null)
        {
            BugSlayerStartExit();
        }
    }

    void BugSlayer_OnEnemyDeath(int score)
    {
        if (gameObject.activeSelf == true && animating == false)
        {
            if (target.gameObject.activeSelf == false)
            {
                CheckNextTarget();
            }
        }
    }

    void CheckNextTarget()
    {
        isChasing = false;
        if (pickTargetRoutine != null)
        {
            StopCoroutine(pickTargetRoutine);
        }

        if (HelperMethods.edibleBugs.Count > 0)
        {
            target = null;
            pickTargetRoutine = StartCoroutine(PickATarget());
            moveComponent.moveSpeed = baseSpeed;
            movementRoutine = StartCoroutine(StartMovementRoutine());
        }
        else
        {
            BugSlayerStartExit();
        }
    }
}
