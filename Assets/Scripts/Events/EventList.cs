using UnityEngine;
using UnityEngine.Events;

public class EventList : MonoBehaviour
{
    public static UnityAction<int> enemyDeath;
    public static UnityAction enemyLeft;
    public static UnityAction projectileHit;
    public static UnityAction waveStarted;
    public static UnityAction playerDeath;

    void Start()
    {
        EventBroker.StartListening("Enemy Death", enemyDeath);
        EventBroker.StartListening("Enemy Left", enemyLeft);
        EventBroker.StartListening("Projectile Hit", projectileHit);
        EventBroker.StartListening("Wave Started", waveStarted);
    }

    void OnDisable()
    {
        EventBroker.StopListening("Enemy Death", enemyDeath);
        EventBroker.StartListening("Enemy Left", enemyLeft);
        EventBroker.StopListening("Projectile Hit", projectileHit);
        EventBroker.StopListening("Wave Started", waveStarted);
    }
}
