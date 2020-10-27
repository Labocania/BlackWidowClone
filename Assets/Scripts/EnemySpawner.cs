using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    public WaveData wave;

    InsectPool mosquitoPool;
    UnityAction enemyDeath;
    int enemyCount;

    void Awake()
    {
        mosquitoPool = GetComponent<InsectPool>();
        enemyCount = wave.enemyQuantity[Enemies.Mosquito];
        mosquitoPool.initialSize = enemyCount;
        mosquitoPool.maximumSize = enemyCount;
        enemyDeath += onEnemyDeath;
    }

    void Start()
    {
        mosquitoPool.GetObject().Spawn(GetRandomSpawnPoint(), Quaternion.identity);
        EventBroker.StartListening("Enemy Death", enemyDeath);
    }


    Vector2 GetRandomSpawnPoint()
    {
        int[] firstPoint = { -4, 4, 6, -6 };
        int decider = Random.Range(0, 1);

        if (decider == 0)
        {
            return new Vector2(Random.Range(-6, 6), firstPoint[Random.Range(0, 1)]);
        }
        else
        {
            return new Vector2(firstPoint[Random.Range(2, 3)], Random.Range(-4, 4));
        }

    }

    void onEnemyDeath()
    {
        enemyCount--;
    }



    void OnDisable()
    {
        EventBroker.StopListening("Enemy Death", enemyDeath);
    }

}
