using System.Collections;
using UnityEngine;

public class Insect : MonoBehaviour
{
    protected System.Random randomNumber = new System.Random();
    protected MovementComponent moveComponent;
    protected SpawnComponent spawn;
    protected PolygonCollider2D polyCollider;
    protected SpriteRenderer baseSprite;
    protected Coroutine movementRoutine;
    protected Color currentColor;
    public float baseSpeed;
    protected int score;
    protected bool animating = false;
    protected bool wasShot = false;
    protected bool flashing = false;

    protected virtual void Awake()
    {
        EventList.waveChanged += Insect_OnWaveChange;
        moveComponent = GetComponent<MovementComponent>();
        spawn = GetComponent<SpawnComponent>();
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
        EventList.playerDeath += OnPlayerDeath;
    }

    protected virtual void OnDisable()
    {
        if (movementRoutine != null)
        {
            StopCoroutine(movementRoutine);
        }
        EventList.playerDeath -= OnPlayerDeath;
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

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("BugSlayer") && flashing == true)
        {
            Die();
            flashing = false;
            return;
        }

        if (collision.gameObject.CompareTag("Projectile"))
        {
            Die();
            spawn?.Spawn();
            return;
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

    protected virtual void OnPlayerDeath()
    {
        RunAway();
    }

    protected void RunAway(bool special = false)
    {
        if (gameObject.activeSelf == true)
        {
            if (special)
            {
                baseSprite.color = ColorList.colors[(int)ColorNames.Yellow];
                gameObject.layer = 16; // Special Escape.
            }
            else
            {
                polyCollider.enabled = false;
            }

            animating = true;
            StopCoroutine(movementRoutine);
            Vector3 direction = transform.position - Vector3.zero;
            moveComponent.RotateTowards(-direction);
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
}
