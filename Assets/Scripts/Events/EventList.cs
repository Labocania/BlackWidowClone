using UnityEngine;
using UnityEngine.Events;

public class EventList : MonoBehaviour
{
    public static UnityAction<int> enemyDeath;
    public static UnityAction<int> grubCollect;
    public static UnityAction<string> enemyLeft;
    public static UnityAction projectileHit;
    public static UnityAction waveStarted;
    public static UnityAction waveChanged;
    public static UnityAction playerDeath;
    public static UnityAction gameOver;
}
