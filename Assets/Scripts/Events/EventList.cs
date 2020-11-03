using UnityEngine;
using UnityEngine.Events;

public class EventList : MonoBehaviour
{
    public static UnityAction<int> enemyDeath;
    public static UnityAction projectileHit;

    void Start()
    {
        EventBroker.StartListening("Enemy Death", enemyDeath);
    }

    void OnDisable()
    {
        EventBroker.StopListening("Enemy Death", enemyDeath);
    }
}
