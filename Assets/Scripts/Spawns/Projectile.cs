using UnityEngine;

public class Projectile : MonoBehaviour
{
	public float projectileSpeed;

	// If the shooter fired this projectile we assign him here so we can subtract from
	// his active projectile count when the projectile is destroyed.
	[HideInInspector]
	public ShootingComponent shooter;


	SpriteRenderer sprite;

    void OnBecameInvisible()
    {
		shooter.activeProjectiles--;
		gameObject.SetActive(false);
	}

    void Start()
    {
		sprite = GetComponent<SpriteRenderer>();   
    }

    void FixedUpdate()
    {
        if (gameObject.activeSelf)
        {
			sprite.color = ColorList.RandomColor();
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
		// Subtracts from shooter active projectiles.
		if (shooter != null) 
		{
			shooter.activeProjectiles--;
			gameObject.SetActive(false);
		}
	}
}
