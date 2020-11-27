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
    protected SpriteRenderer baseSprite;
    protected Coroutine movementRoutine;
    protected Color currentColor;
    protected float baseSpeed;
    protected int score;
    protected bool animating = false;
    protected bool wasShot;

    protected virtual void Awake()
    {
        moveComponent = GetComponent<MovementComponent>();
        baseSprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
        currentColor = baseSprite.color;
        polyCollider = GetComponent<PolygonCollider2D>();
        if (polyCollider == null)
        {
            polyCollider = GetComponentInChildren<PolygonCollider2D>(true);
        }
        baseSpeed = moveComponent.moveSpeed;
        waitTimes.Add(new WaitForSeconds(0.1f));
    }

    protected virtual void FixedUpdate()
    {
        if (gameObject.activeSelf == true)
        {
            moveComponent.TransformMove(transform.up);
        }
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
        baseSprite.color = currentColor;
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

    public void FlashColors()
    {
        StartCoroutine(FlashRoutine());
    }

    IEnumerator FlashRoutine()
    {      
        while (gameObject.activeSelf == true && !animating)
        {
            baseSprite.color = Color.white;
            yield return waitTimes[0];
            baseSprite.color = currentColor;
            yield return waitTimes[0];
        }
    }

    protected virtual void Chase() { }

    protected virtual void Shoot() { }

    protected virtual void Spawn() { }

}
