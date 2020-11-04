using UnityEngine;

public class Mosquito : Insect
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            Die();
        }
    }

    public override void Die()
    {
        gameObject.SetActive(false);
        EventBroker.TriggerEvent("Enemy Death", score);
    }

}
