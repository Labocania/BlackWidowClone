using UnityEngine;

public class Projectile : MonoBehaviour
{
	public float projectileSpeed;
	SpriteRenderer sprite;
    Rigidbody2D rb;

    void OnBecameInvisible()
    {
        gameObject.SetActive(false);
        EventBroker.TriggerEvent("Projectile Hit");
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
        if (collision.gameObject.CompareTag("Enemy"))
        {
			Hit();
        }
    }

    public void Hit() 
	{
        gameObject.SetActive(false);
        EventBroker.TriggerEvent("Projectile Hit");
    }
}
