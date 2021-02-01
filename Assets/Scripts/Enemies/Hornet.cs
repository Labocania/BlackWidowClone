
using UnityEngine;

public class Hornet : Beetle
{
    SpawnComponent eggSpawn;
    protected override void Awake()
    {
        base.Awake();
        eggSpawn = GetComponent<SpawnComponent>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (playerChaser.GrubTarget != null)
        {
            if (playerChaser.GrubTarget == collision.transform)
            {
                Destroy(collision.gameObject);
                eggSpawn.SpawnOnCapture();
                playerChaser.StopBugChase();
                playerChaser.CheckNextTarget();
                return;
            }
        }
    }
}
