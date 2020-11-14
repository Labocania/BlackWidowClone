using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour 
{
    public WaveData wave;

    WaitForSeconds spawnInterval;
    WaitUntil waveStartCondition;

    const int MAX_ENEMIES_ON_SCREEN = 11;
    int enemiesOnScreen;
    int killableEnemiesOnScreen;
    bool isSpawing = false;

    List<GameObjectPool> objectPoolComponents;
    GameObjectPool currentPool;
    Dictionary<GameObject, GameObjectPool> spawnDictionary = new Dictionary<GameObject, GameObjectPool>();

    System.Random random = new System.Random();
    Coroutine spawingRoutine;

    void Awake()
    {
        EventList.waveStarted += EnemySpawn_OnWaveStarted;
        EventList.enemyDeath += EnemySpawn_OnEnemyDeath;
        EventList.playerDeath += EnemySpawn_OnPlayerDeath;
        EventList.enemyLeft += EnemySpawn_OnEnemyLeft;
    }

    void Start()
    {
        ReadWaveData();
        spawnInterval = new WaitForSeconds(wave.spawingSpeed);
        waveStartCondition = new WaitUntil(() => enemiesOnScreen == 0 && killableEnemiesOnScreen == 0);
    }

    void Update()
    {
        if (killableEnemiesOnScreen == 0)
        {
            // Start remaining enemies exit routine
            // Trigger Wave Change event
        }
    }

    void ReadWaveData()
    {
        foreach (KeyValuePair<GameObject, int> enemy in wave.totalQuantity)
        {
            if (!spawnDictionary.ContainsKey(enemy.Key))
            {
                spawnDictionary[enemy.Key] = gameObject.AddComponent<GameObjectPool>();
                spawnDictionary[enemy.Key].prefab = enemy.Key;
                spawnDictionary[enemy.Key].spawnTotal = enemy.Value;
                spawnDictionary[enemy.Key].maximumSize = MAX_ENEMIES_ON_SCREEN;

            }
            else
            {
                spawnDictionary[enemy.Key].spawnTotal = enemy.Value;
            }
        }

        objectPoolComponents = new List<GameObjectPool>(gameObject.GetComponents<GameObjectPool>());
    }

    IEnumerator StartSpawningRoutine()
    {
        while (isSpawing)
        {
            if (objectPoolComponents.Count > 0 && enemiesOnScreen < MAX_ENEMIES_ON_SCREEN)
            {
                yield return spawnInterval;
                SpawnEnemy();

                if (enemiesOnScreen == MAX_ENEMIES_ON_SCREEN)
                {
                    isSpawing = false;
                }
            }
            else
            {
                isSpawing = false;
            }
        }
    }

    void SpawnEnemy()
    {
        currentPool = objectPoolComponents[random.Next(objectPoolComponents.Count)];
        if (currentPool.spawnTotal > 0)
        {
            GameObject obj = currentPool.GetObject();

            obj.transform.position = GetRandomSpawnPoint();
            obj.transform.up = Vector3.zero - obj.transform.position;

            enemiesOnScreen++;

            if (currentPool.prefab.CompareTag("KillableEnemy"))
            {
                killableEnemiesOnScreen++;
            }
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

    void EnemySpawn_OnEnemyDeath(int score)
    {
        if (enemiesOnScreen > 0 || killableEnemiesOnScreen > 0)
        {
            enemiesOnScreen--;
            killableEnemiesOnScreen--;
        }

        if (!isSpawing)
        {
            isSpawing = true;
            spawingRoutine = StartCoroutine(StartSpawningRoutine());
        }

    }

    IEnumerator WaveStartRoutine()
    {
        yield return waveStartCondition;
        isSpawing = true;
        spawingRoutine = StartCoroutine(StartSpawningRoutine());
    }

    void EnemySpawn_OnWaveStarted()
    {
        StartCoroutine(WaveStartRoutine());
    }

    void EnemySpawn_OnPlayerDeath()
    {
        if (isSpawing)
        {
            isSpawing = false;
            StopCoroutine(spawingRoutine);
        }     
    }

    void EnemySpawn_OnEnemyLeft()
    {
        currentPool.spawnTotal++;
        enemiesOnScreen--;
        killableEnemiesOnScreen--;
    }
}
