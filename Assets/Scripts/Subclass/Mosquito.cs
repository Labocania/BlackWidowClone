using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mosquito : Insect
{
    MovementComponent moveComponent;
    PolygonCollider2D collider;

    List<Vector3> rotationAngles = new List<Vector3>();
    List<WaitForSeconds> waitTimes = new List<WaitForSeconds>();
    System.Random randomIndex = new System.Random();
    Coroutine movementRoutine;
    bool animating = false;
    bool wasShot;
    float speed;

    void Awake()
    {
        EventList.playerDeath += onPlayerDeath;

        moveComponent = GetComponent<MovementComponent>();
        collider = GetComponent<PolygonCollider2D>();
        if (collider == null)
        {
            collider = GetComponentInChildren<PolygonCollider2D>();
        }
        speed = moveComponent.moveSpeed;

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
    }

    void FixedUpdate()
    {
        if (gameObject.activeSelf == true)
        {
            moveComponent.TransformMove(transform.up);       
        }
    }

    void OnEnable()
    {
        movementRoutine = StartCoroutine(StartMovementRoutine());
        EventBroker.StartListening("Player Death", EventList.playerDeath);
    }

    void OnDisable()
    {
        StopCoroutine(movementRoutine);
        EventBroker.StopListening("Player Death", EventList.playerDeath);
    }

    IEnumerator StartMovementRoutine()
    {
        while (gameObject.activeSelf == true && animating == false)
        {
            yield return waitTimes[randomIndex.Next(waitTimes.Count)];
            yield return moveComponent.TransformRotate(rotationAngles[randomIndex.Next(rotationAngles.Count)], 3f);
        }
    }
   
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            wasShot = true;
            Die();
        }
    }

    void OnBecameInvisible()
    {
        gameObject.SetActive(false);
        collider.enabled = true;
        animating = false;
        moveComponent.moveSpeed = speed;

        if (!wasShot)
        {
            EventBroker.TriggerEvent("Enemy Left");
        }

        wasShot = false;
    }

    public override void Die()
    {
        gameObject.SetActive(false);
        EventBroker.TriggerEvent("Enemy Death", score);
    }

    void onPlayerDeath()
    {
        collider.enabled = false;
        animating = true;

        StopCoroutine(movementRoutine);

        Vector3 direction = transform.position - Vector3.zero;
        Quaternion rotation = Quaternion.LookRotation(-direction);
        Vector3 eulerAngles = rotation.eulerAngles;
        moveComponent.TransformRotate(eulerAngles, 0.2f);
        moveComponent.moveSpeed += 3;
    }

}
