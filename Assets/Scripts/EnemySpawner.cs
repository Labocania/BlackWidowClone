using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour 
{
    const int MAX_ENEMIES = 11;
    int enemiesOnScreen;
    public WaveData wave;
    UnityAction enemyDeath;
    GameObjectPool[] pools;
    Dictionary<GameObject, int> spawnDictionary = new Dictionary<GameObject, int>();

    void Awake()
    {
        enemyDeath += onEnemyDeath;
    }

    void Start()
    {
        pools = gameObject.GetComponents<GameObjectPool>();
        ReadComponentData();
        ReadWaveData();
        EventBroker.StartListening("Enemy Death", enemyDeath);
    }

    void ReadComponentData()
    {
        foreach (var component in pools)
        {
            spawnDictionary.Add(component.prefab, 0);
        }
    }

    void ReadWaveData()
    {
        foreach (var enemy in wave.enemyQuantity)
        {
            spawnDictionary[enemy.Key] = enemy.Value;
        }
    }

    void SpawnEnemies()
    {

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
        //insectCount--;
    }

    void OnDisable()
    {
        EventBroker.StopListening("Enemy Death", enemyDeath);
    }

}
