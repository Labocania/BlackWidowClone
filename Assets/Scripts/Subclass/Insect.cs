using System.Collections;
using UnityEngine;

public class Insect : MonoBehaviour
{
    protected MovementComponent moveComponent;
    protected PolygonCollider2D polyCollider;
    protected Coroutine movementRoutine;
    protected float speed;
    protected int score;
    protected bool animating = false;
    protected bool wasShot;

    protected virtual void Awake()
    {
        EventList.playerDeath += onPlayerDeath;


        moveComponent = GetComponent<MovementComponent>();
        polyCollider = GetComponent<PolygonCollider2D>();
        if (polyCollider == null)
        {
            polyCollider = GetComponentInChildren<PolygonCollider2D>();
        }
        speed = moveComponent.moveSpeed;
    }

    protected virtual void OnEnable()
    {
        movementRoutine = StartCoroutine(StartMovementRoutine());
        //EventBroker.StartListening("Player Death", EventList.playerDeath);
    }

    protected virtual void OnDisable()
    {
        StopCoroutine(movementRoutine);
        //EventBroker.StopListening("Player Death", EventList.playerDeath);
    }

    protected virtual void OnBecameInvisible()
    {
        gameObject.SetActive(false);
        polyCollider.enabled = true;
        animating = false;
        moveComponent.moveSpeed = speed;

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
        EventBroker.TriggerEvent("Enemy Death", score);
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

    public virtual void Chase() { }

    public virtual void Eat() { }

    public virtual void Shoot() { }

}
