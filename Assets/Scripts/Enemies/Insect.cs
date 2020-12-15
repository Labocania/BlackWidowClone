using System.Collections;
using UnityEngine;

public class Insect : MonoBehaviour
{
    protected System.Random randomNumber = new System.Random();
    protected MovementComponent moveComponent;
    protected PolygonCollider2D polyCollider;
    protected SpriteRenderer baseSprite;
    protected Coroutine movementRoutine;
    protected Color currentColor;
    public float baseSpeed;
    protected int score;
    protected bool animating = false;
    protected bool wasShot;
    protected bool flashing;

    protected virtual void Awake()
    {
        EventList.waveChanged += Insect_OnWaveChange;
        moveComponent = GetComponent<MovementComponent>();
        baseSprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
        currentColor = baseSprite.color;
        polyCollider = GetComponent<PolygonCollider2D>();
        if (polyCollider == null)
        {
            polyCollider = GetComponentInChildren<PolygonCollider2D>(true);
        }
        moveComponent.MoveSpeed = baseSpeed;
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
        moveComponent.MoveSpeed = baseSpeed;

        if (wasShot == false)
        {
            string name = gameObject.name.Replace("(Clone)", "");
            EventList.enemyLeft.Invoke(name);
        }
        wasShot = false;
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

    protected virtual void onPlayerDeath()
    {
        RunAway();
    }

    protected void RunAway(bool bugSlayer = false)
    {
        if (gameObject.activeSelf == true)
        {
            if (bugSlayer)
            {
                gameObject.layer = 16; // Special Escape.
            }
            else
            {
                polyCollider.enabled = false;
            }

            animating = true;
            StopCoroutine(movementRoutine);
            Vector3 direction = transform.position - Vector3.zero;
            Quaternion rotation = Quaternion.LookRotation(-direction);
            Vector3 eulerAngles = rotation.eulerAngles;
            moveComponent.TransformRotate(eulerAngles, 0.2f);
            moveComponent.MoveSpeed += 3;
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
            flashing = true;
            baseSprite.color = Color.white;
            yield return HelperMethods.GetWaitTime(0.1f);
            baseSprite.color = currentColor;
            yield return HelperMethods.GetWaitTime(0.1f);
        }

        flashing = false;
    }

    void Insect_OnWaveChange()
    {
        baseSpeed += 0.5f;
    }

    protected virtual void Shoot() { }

    protected virtual void Spawn() { }

}
