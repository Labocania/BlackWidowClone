using UnityEngine;
using UnityEngine.Events;

public class EventList : MonoBehaviour
{
    public static UnityAction<int> enemyDeath;
    public static UnityAction projectileHit;
    public static UnityAction animationFinished;
    public static UnityAction playerDeath;

    void Start()
    {
        EventBroker.StartListening("Enemy Death", enemyDeath);
        EventBroker.StartListening("Projectile Hit", projectileHit);
        EventBroker.StartListening("AnimationFinished", animationFinished);

    }

    void OnDisable()
    {
        EventBroker.StopListening("Enemy Death", enemyDeath);
        EventBroker.StopListening("Projectile Hit", projectileHit);
        EventBroker.StopListening("AnimationFinished", animationFinished);

    }
}
