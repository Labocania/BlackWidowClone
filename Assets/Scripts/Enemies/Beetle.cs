using UnityEngine;

public class Beetle : Insect
{
    PlayerChaser playerChaser;
    BounceComponent bounce;

    protected override void Awake()
    {
        base.Awake();
        playerChaser = GetComponent<PlayerChaser>();
        bounce = GetComponent<BounceComponent>();
        bounce.ResetBounces(3);
        spawn = GetComponent<SpawnComponent>();

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
        HelperMethods.RemoveEdibleBug(transform);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (playerChaser.IsChasing)
        {
            playerChaser.StartChase();
        }
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        GameObject obj = collision.gameObject;
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
            }
        }
    }
}
