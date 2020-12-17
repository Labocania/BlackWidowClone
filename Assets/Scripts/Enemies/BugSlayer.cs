using System.Collections;
using UnityEngine;

public class BugSlayer : Insect
{
    BugChaser bugChaser;

    protected override void Awake()
    {
        base.Awake();
        score = 1000;
        bugChaser = GetComponent<BugChaser>();
        bugChaser.moveComp = moveComponent;

        EventList.noTargets += BugSlayer_OnNoTargets;
        EventList.enemyDeath += BugSlayer_OnEnemyDeath;
        EventList.grubCollect += BugSlayer_OnBugCollect;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        bugChaser.CheckNextTarget();
    }

    protected override void OnDisable() => base.OnDisable();
    protected override void OnBecameInvisible() // fix this
    {
        bugChaser.StopAllChases();
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
        baseSprite.color = currentColor;
        moveComponent.MoveSpeed = baseSpeed;
        polyCollider.enabled = true;
        gameObject.layer = 8; // BugSlayer
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

    protected override void OnPlayerDeath()
    {
        bugChaser.StopAllChases();
        base.OnPlayerDeath();
    }

    void BugSlayerStartExit()
    {
        RunAway(true);
    }


    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (bugChaser.IsChasing)
        {
            bugChaser.StartChase();
        }
    }

    protected override IEnumerator StartMovementRoutine()
    {
        while (gameObject.activeSelf == true && animating == false)
        {
            yield return moveComponent.TransformRotate(HelperMethods.GetRandomAngle(20f, -20f), 1f);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject obj = collision.gameObject;
        if (obj.CompareTag("Projectile"))
        {
            Die();
            return;
        }

        if (bugChaser.GrubTarget != null && obj == bugChaser.GrubTarget.gameObject)
        {
            bugChaser.StopAllChases();
            bugChaser.CheckNextTarget();
            movementRoutine = StartCoroutine(StartMovementRoutine());
            return;
        }

        if (obj.CompareTag("Edge"))
        {
            moveComponent.RotateTowards(Vector2.zero);
        }

        if (obj.CompareTag("Player"))
        {
            bugChaser.StopAllChases();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (bugChaser.GrubTarget != null && collision.gameObject == bugChaser.GrubTarget.gameObject)
        {
            Destroy(collision.gameObject);
            bugChaser.StopAllChases();
            bugChaser.CheckNextTarget();
            movementRoutine = StartCoroutine(StartMovementRoutine());
        }
    }

    void BugSlayer_OnNoTargets()
    {
        bugChaser.StopAllChases();
        baseSprite.color = ColorList.colors[(int)ColorNames.Yellow];
        wasShot = true;
        if (this != null)
        {
            BugSlayerStartExit();
        }
    }

    void BugSlayer_OnEnemyDeath(int score)
    {
        if (gameObject.activeSelf == true && animating == false)
        {
            if (bugChaser.GrubTarget != null && bugChaser.GrubTarget.gameObject.activeSelf == false)
            {
                bugChaser.StopAllChases();
                bugChaser.CheckNextTarget();
                movementRoutine = StartCoroutine(StartMovementRoutine());
            }
        }
    }

    void BugSlayer_OnBugCollect(int score)
    {
        if (gameObject.activeSelf == true && animating == false)
        {
            if (bugChaser.GrubTarget != null && bugChaser.GrubTarget.gameObject.activeSelf == false)
            {
                bugChaser.StopAllChases();
                bugChaser.CheckNextTarget();
                movementRoutine = StartCoroutine(StartMovementRoutine());
            }
        }
    }

    protected override void Die()
    {
        wasShot = true;
        gameObject.SetActive(false);
    }
}
