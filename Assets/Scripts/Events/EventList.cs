using UnityEngine;
using UnityEngine.Events;

public class EventList : MonoBehaviour
{
    public static UnityAction<int> enemyDeath;
    public static UnityAction projectileHit;

    void Start()
    {
        EventBroker.StartListening("Enemy Death", enemyDeath);
        EventBroker.StartListening("Projectile Hit", projectileHit);
    }

    void OnDisable()
    {
        EventBroker.StopListening("Enemy Death", enemyDeath);
        EventBroker.StopListening("Projectile Hit", projectileHit);
    }
}
