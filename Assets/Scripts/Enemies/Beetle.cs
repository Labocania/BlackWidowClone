using System.Collections;
using UnityEngine;

public class Beetle : Insect
{
    public GameObject grub;
    PlayerChaser playerChaser;
    BounceComponent bounce;

    protected override void Awake()
    {
        base.Awake();
        playerChaser = GetComponent<PlayerChaser>();
        bounce = GetComponent<BounceComponent>();
        bounce.ResetBounces(3);
        playerChaser.moveComp = moveComponent;

        EventList.grubCollect += Beetle_OnBugCollect;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        playerChaser.CheckNextTarget();
    }

    protected override void OnBecameInvisible()
    {
        base.OnBecameInvisible();
        playerChaser.StopAllChases();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (playerChaser.IsChasing)
        {
            playerChaser.StartChase();
        }
    }

    protected override IEnumerator StartMovementRoutine()
    {
        return base.StartMovementRoutine(); // Implement later.
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject obj = collision.gameObject;
        if (obj.CompareTag("Projectile"))
        {
            Spawn();
            Die();
            return;
        }

        if (obj.CompareTag("Player"))
        {
            playerChaser.StopAllChases();
            return;
        }

        // Edge of the sceen or Green Web
        if (collision.enabled && gameObject.activeSelf == true)
        {
            if (movementRoutine != null)
            {
                StopCoroutine(movementRoutine);
                playerChaser.StopAllChases();
            }

            if (bounce.BounceAmount == 0)
            {
                StartCoroutine(bounce.ResetRoutine());
                //Reset amount with another random number
                bounce.ResetBounces(3);
                playerChaser.SetPlayerTarget();
                playerChaser.CheckNextTarget();
                return;
            }

            StartCoroutine(bounce.BounceRoutine());
            bounce.DecreaseBounce();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (playerChaser.GrubTarget != null)
        {
            if (playerChaser.GrubTarget == collision.transform)
            {
                Destroy(collision.gameObject);
                playerChaser.StopBugChase();
                playerChaser.CheckNextTarget();
                //movementRoutine = StartCoroutine(StartMovementRoutine());
                return;
            }
        }
    }

    protected override void OnPlayerDeath()
    {
        playerChaser.StopAllChases();
        base.OnPlayerDeath();
    }

    void Beetle_OnBugCollect(int score)
    {
        if (this != null && animating == false)
        {
            if (playerChaser.GrubTarget != null && playerChaser.GrubTarget.gameObject.activeSelf == false)
            {
                playerChaser.StopBugChase();
                playerChaser.CheckNextTarget();
                movementRoutine = StartCoroutine(StartMovementRoutine());
            }
        }
    }

    protected override void Spawn()
    {
        Instantiate(grub, transform.position, Quaternion.identity);
    }
}
