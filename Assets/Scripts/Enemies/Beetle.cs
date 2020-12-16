using System.Collections;
using UnityEngine;

public class Beetle : Insect
{
    public GameObject grub;
    PlayerChaser playerChaser;

    protected override void Awake()
    {
        base.Awake();
        playerChaser = GetComponent<PlayerChaser>();
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

    void Beetle_OnBugCollect(int score)
    {
        if (gameObject.activeSelf == true && animating == false)
        {
            if (playerChaser.GrubTarget != null && playerChaser.GrubTarget.gameObject.activeSelf == false)
            {
                playerChaser.StopAllChases();
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
