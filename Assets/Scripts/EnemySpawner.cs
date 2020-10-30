using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour 
{
    const int MAX_ENEMIES_ON_SCREEN = 11;
    int enemiesOnScreen = 0;
    public WaveData wave;
    UnityAction enemyDeath;
    List<GameObjectPool> objectPoolComponents;
    System.Random random = new System.Random();
    Dictionary<GameObject, GameObjectPool> spawnDictionary = new Dictionary<GameObject, GameObjectPool>();

    void Start()
    {
        ReadWaveData();
        enemyDeath += onEnemyDeath;
        EventBroker.StartListening("Enemy Death", enemyDeath);
        SpawnEnemies();
    }

    void Update()
    {
        if (objectPoolComponents.Count > 0 && enemiesOnScreen <= MAX_ENEMIES_ON_SCREEN)
        {
            SpawnEnemies();
        }
    }


    void ReadWaveData()
    {
        foreach (KeyValuePair<GameObject, int> enemy in wave.enemyQuantity)
        {
            if (!spawnDictionary.ContainsKey(enemy.Key))
            {
                spawnDictionary[enemy.Key] = gameObject.AddComponent<GameObjectPool>();
                spawnDictionary[enemy.Key].prefab = enemy.Key;
                spawnDictionary[enemy.Key].spawnCounter = enemy.Value;
                spawnDictionary[enemy.Key].maximumSize = enemy.Value;
            }
            else
            {
                spawnDictionary[enemy.Key].spawnCounter = enemy.Value;
            }
        }

        objectPoolComponents = new List<GameObjectPool>(gameObject.GetComponents<GameObjectPool>());
    }

    void SpawnEnemies()
    {
        GameObjectPool pool = objectPoolComponents[random.Next(objectPoolComponents.Count)];
        if (pool.spawnCounter > 0)
        {
            pool.GetObject().transform.position = GetRandomSpawnPoint();
            enemiesOnScreen++;
        }
        else
        {
            objectPoolComponents.Remove(pool);
        }
    }


    Vector2 GetRandomSpawnPoint()
    {
        int[] firstPoint = { -4, 4, 6, -6 };
        int decider = 0;

        if (decider == 0)
        {
            decider = 1;
            return new Vector2(Random.Range(-6, 7), firstPoint[Random.Range(0, 2)]);
        }
        else
        {
            decider = 0;
            return new Vector2(firstPoint[Random.Range(2, 4)], Random.Range(-4, 5));
        }

    }

    void onEnemyDeath()
    {
        enemiesOnScreen--;
    }

    void OnDisable()
    {
        EventBroker.StopListening("Enemy Death", enemyDeath);
    }

}
