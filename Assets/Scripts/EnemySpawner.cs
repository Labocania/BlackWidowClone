using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour 
{
    public WaveData wave;
    UnityAction enemyDeath;
    int insectCount;
    List<GameObjectPool> pools = new List<GameObjectPool>();

    void Awake()
    {
        ReadWaveData();
        enemyDeath += onEnemyDeath;
    }

    void Start()
    {
        pools[0].GetObject().transform.position = GetRandomSpawnPoint();
        EventBroker.StartListening("Enemy Death", enemyDeath);
    }

    void ReadWaveData()
    {
        foreach (var enemy in wave.enemyQuantity)
        {
            GameObjectPool pool = gameObject.AddComponent<GameObjectPool>();
            pools.Add(pool);
            pool.prefab = enemy.Key;
        }
    }

    Vector2 GetRandomSpawnPoint()
    {
        int[] firstPoint = { -4, 4, 6, -6 };
        int decider = 0;

        if (decider == 0)
        {
            decider = 1;
            return new Vector2(UnityEngine.Random.Range(-6, 7), firstPoint[UnityEngine.Random.Range(0, 2)]);
        }
        else
        {
            decider = 0;
            return new Vector2(firstPoint[UnityEngine.Random.Range(2, 4)], UnityEngine.Random.Range(-4, 5));
        }

    }

    void onEnemyDeath()
    {
        insectCount--;
    }

    void OnDisable()
    {
        EventBroker.StopListening("Enemy Death", enemyDeath);
    }

}
