using UnityEngine;

public class Projectile : MonoBehaviour
{
	public float projectileSpeed;
	SpriteRenderer sprite;
    Rigidbody2D rb;

    void OnBecameInvisible()
    {
        EventBroker.TriggerEvent("Projectile Hit");
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
        if (collision.gameObject.CompareTag("Enemy"))
        {
			Hit();
        }
    }

    public void Hit() 
	{
        EventBroker.TriggerEvent("Projectile Hit");
        gameObject.SetActive(false);
    }
}
