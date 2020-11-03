using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour 
{
    public WaveData wave;

    WaitForSeconds spawnInterval;
    const int MAX_ENEMIES_ON_SCREEN = 10;
    int enemiesOnScreen = 0;
    bool spawing;

    List<GameObjectPool> objectPoolComponents;
    GameObjectPool currentPool;
    Dictionary<GameObject, GameObjectPool> spawnDictionary = new Dictionary<GameObject, GameObjectPool>();

    System.Random random = new System.Random();

    void Awake()
    {
        EventList.enemyDeath += onEnemyDeath;
    }

    void Start()
    {
        ReadWaveData();
        spawnInterval = new WaitForSeconds(wave.spawingSpeed);
        StartCoroutine(StartSpawningRoutine());
    }

    void Update()
    {
        if (spawing == false && objectPoolComponents.Count > 0 && enemiesOnScreen <= MAX_ENEMIES_ON_SCREEN)
        {
            StartCoroutine(StartSpawningRoutine());
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

    IEnumerator StartSpawningRoutine()
    {
        spawing = true;
        while (objectPoolComponents.Count > 0 && enemiesOnScreen <= MAX_ENEMIES_ON_SCREEN)
        {
            SpawnEnemy();
            yield return spawnInterval;
        }
        spawing = false;
    }
    void SpawnEnemy()
    {
        currentPool = objectPoolComponents[random.Next(objectPoolComponents.Count)];
        if (currentPool.spawnCounter > 0)
        {
            currentPool.GetObject().transform.position = GetRandomSpawnPoint();
            enemiesOnScreen++;
        }
        else
        {
            objectPoolComponents.Remove(currentPool);
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

    void onEnemyDeath(int score)
    {
        if (enemiesOnScreen > 0)
        {
            enemiesOnScreen--;
        }
    }
}
