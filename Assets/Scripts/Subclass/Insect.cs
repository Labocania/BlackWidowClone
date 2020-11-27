using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Insect : MonoBehaviour
{
    protected List<Vector3> rotationAngles = new List<Vector3>();
    protected List<WaitForSeconds> waitTimes = new List<WaitForSeconds>();
    protected System.Random randomNumber = new System.Random();
    protected MovementComponent moveComponent;
    protected PolygonCollider2D polyCollider;
    protected Coroutine movementRoutine;
    protected float baseSpeed;
    protected int score;
    protected bool animating = false;
    protected bool wasShot;

    protected virtual void Awake()
    {
        moveComponent = GetComponent<MovementComponent>();
        polyCollider = GetComponent<PolygonCollider2D>();
        if (polyCollider == null)
        {
            polyCollider = GetComponentInChildren<PolygonCollider2D>();
        }
        baseSpeed = moveComponent.moveSpeed;
    }

    protected virtual void OnEnable()
    {
        movementRoutine = StartCoroutine(StartMovementRoutine());
        EventList.playerDeath += onPlayerDeath;
    }

    protected virtual void OnDisable()
    {
        if (movementRoutine != null)
        {
            StopCoroutine(movementRoutine);
        }
        EventList.playerDeath -= onPlayerDeath;
    }

    protected virtual void OnBecameInvisible()
    {
        gameObject.SetActive(false);
        polyCollider.enabled = true;
        animating = false;
        moveComponent.moveSpeed = baseSpeed;

        if (!wasShot)
        {
            string name = gameObject.name.Replace("(Clone)", "");
            EventList.enemyLeft.Invoke(name);
        }
        else
        {
            wasShot = false;
        }
    }

    protected virtual void Die()
    {
        wasShot = true;
        gameObject.SetActive(false);
        EventList.enemyDeath.Invoke(score);
    }

    protected virtual IEnumerator StartMovementRoutine()
    {
        yield return null;
    }

    void onPlayerDeath()
    {
        if (gameObject.activeSelf == true)
        {
            polyCollider.enabled = false;
            animating = true;

            StopCoroutine(movementRoutine);

            Vector3 direction = transform.position - Vector3.zero;
            Quaternion rotation = Quaternion.LookRotation(-direction);
            Vector3 eulerAngles = rotation.eulerAngles;
            moveComponent.TransformRotate(eulerAngles, 0.2f);
            moveComponent.moveSpeed += 3;
        }
    }

    protected virtual void Chase() { }

    protected virtual void Eat() { }

    protected virtual void Shoot() { }

    protected virtual void Spawn() { }

}
