using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class MovementComponent : MonoBehaviour
{
    Rigidbody2D rBody;
    Animator movementAnim;
    public float moveSpeed = 5f;

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

    public void Move(Vector2 direction)
    {
        rBody.velocity = new Vector2(direction.x, direction.y) * moveSpeed;
        UpdatePlayerAnimation(direction);
    }

    void UpdatePlayerAnimation(Vector2 direction)
    {
        movementAnim.SetFloat("MoveY", direction.y);
        movementAnim.SetFloat("MoveX", direction.x);
        movementAnim.SetFloat("Speed", direction.sqrMagnitude);
    }

    public void Rotate(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

}
