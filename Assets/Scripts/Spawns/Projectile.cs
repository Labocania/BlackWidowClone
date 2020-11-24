using UnityEngine;

public class Projectile : MonoBehaviour
{
	public float projectileSpeed;
	SpriteRenderer sprite;
    Rigidbody2D rb;
    System.Random randomNumber = new System.Random();
    float[] rotationAngles = { 40, 90, -40, -90, 0};
    bool isAngled = false;

    void OnBecameInvisible()
    {
        EventList.projectileHit.Invoke();
        isAngled = false;
        gameObject.SetActive(false);   
    }

    void Start()
    {
		sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (gameObject.activeSelf)
        {
			sprite.color = ColorList.RandomColor();
            rb.velocity = transform.up * projectileSpeed;
        }    
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("KillableEnemy"))
        {
			Hit();
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Grub"))
        {
            if (!isAngled)
            {
                TwistAngle();
                isAngled = true;
            }
        }
    }

    void Hit() 
	{
        EventList.projectileHit.Invoke();
        isAngled = false;
        gameObject.SetActive(false);
    }

    void TwistAngle()
    {
        transform.Rotate(Vector3.forward, rotationAngles[randomNumber.Next(rotationAngles.Length)]);
    }
}
