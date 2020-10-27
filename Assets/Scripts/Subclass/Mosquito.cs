using UnityEngine;

public class Mosquito : Insect
{
    public override void Spawn(Vector3 position, Quaternion rotation)
    {
        transform.position = position;
        transform.rotation = rotation;
    }

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
        EventBroker.TriggerEvent("Enemy Death");
    }

}
