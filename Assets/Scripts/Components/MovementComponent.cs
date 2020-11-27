using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovementComponent : MonoBehaviour
{
    Rigidbody2D rBody;
    Animator movementAnim;
    public float moveSpeed = 5f;
    WaitForFixedUpdate waitTime;

    // Start is called before the first frame update
    void Awake()
    {
        rBody = GetComponent<Rigidbody2D>();
        movementAnim = GetComponent<Animator>();
        if (movementAnim == null)
        {
            movementAnim = GetComponentInChildren<Animator>();
        }
    }

    void Start()
    {
        waitTime = new WaitForFixedUpdate();
    }

    public void TransformMove(Vector2 direction)
    {
        transform.Translate(direction * Time.fixedDeltaTime * moveSpeed, Space.World);

        if (movementAnim != null)
        {
            UpdateAnimation(direction);
        }
    }

    public void RigidBodyMove(Vector2 direction)
    {
        //rBody.velocity = new Vector2(direction.x, direction.y) * moveSpeed;
        rBody.AddForce(new Vector2(direction.x, direction.y) * moveSpeed);

        if (movementAnim != null)
        {
            UpdateAnimation(direction);
        }
    }

    public void TransformSnapRotate(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public IEnumerator TransformRotate(Vector3 direction, float timeToRotate)
    {
        float currentTime = 0;
        while (currentTime < timeToRotate)
        {
            transform.Rotate(direction * (Time.fixedDeltaTime / timeToRotate));
            currentTime += Time.fixedDeltaTime;
            yield return waitTime;
        }
        yield break;
    }

    public void MoveTowards(Vector2 target)
    {
        transform.position = Vector2.MoveTowards(transform.position, target, moveSpeed * Time.fixedDeltaTime);
    }

    public void RotateTowards(Vector2 target)
    {
        Vector2 direction = (target - (Vector2)transform.position).normalized;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        var offset = -90f;
        transform.rotation = Quaternion.Euler(Vector3.forward * (angle + offset));
    }

    void UpdateAnimation(Vector2 direction)
    {
        movementAnim.SetFloat("MoveY", direction.y);
        movementAnim.SetFloat("MoveX", direction.x);
        movementAnim.SetFloat("Speed", direction.sqrMagnitude);
    }

}
