using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour 
{
    WaveData wave;
    float timer;
    WaitUntil waveStartCondition;

    const int MAX_ENEMIES_ON_SCREEN = 11;
    int enemiesOnScreen;
    int killableEnemiesOnScreen;
    int killableEnemies;
    bool isSpawing = false;

    List<string> keyList = new List<string>();
    GameObjectPool currentPool;
    Dictionary<string, GameObjectPool> spawnDictionary = new Dictionary<string, GameObjectPool>();

    System.Random random = new System.Random();

    void Awake()
    {
        LoadWaveData();
        EventList.waveStarted += EnemySpawn_OnWaveStarted;
        EventList.enemyDeath += EnemySpawn_OnEnemyDeath;
        EventList.playerDeath += EnemySpawn_OnPlayerDeath;
        EventList.enemyLeft += EnemySpawn_OnEnemyLeft;
    }

    void Start()
    {
        timer = wave.spawingSpeed;
        waveStartCondition = new WaitUntil(() => enemiesOnScreen == 0 && killableEnemiesOnScreen == 0);
    }

    void Update()
    {
        if (isSpawing)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                if (keyList.Count > 0 && enemiesOnScreen < MAX_ENEMIES_ON_SCREEN)
                {
                    SpawnEnemy();
                }
                else
                {
                    isSpawing = false;
                }
                
                timer = wave.spawingSpeed;
            }
        }
    }

    void LoadWaveData()
    {
        wave = DataReader.instance.LoadWaveData();
        ReadWaveData();
    }

    void ReloadWaveData()
    {
        wave = DataReader.instance.LoadWaveData();
        ReadWaveData();
    }

    void ReadWaveData()
    {
        foreach (KeyValuePair<GameObject, int> enemy in wave.totalQuantity)
        {
            if (!spawnDictionary.ContainsKey(enemy.Key.name))
            {
                spawnDictionary[enemy.Key.name] = gameObject.AddComponent<GameObjectPool>();
                spawnDictionary[enemy.Key.name].prefab = enemy.Key;
                spawnDictionary[enemy.Key.name].maximumSize = MAX_ENEMIES_ON_SCREEN;
            }

            if (enemy.Key.CompareTag("KillableEnemy"))
            {
                killableEnemies += enemy.Value;
            }
            spawnDictionary[enemy.Key.name].spawnTotal = enemy.Value;

            keyList.Add(enemy.Key.name);

        }
    }

    void SpawnEnemy()
    {
        string randomKey = keyList[random.Next(keyList.Count)];
        currentPool = spawnDictionary[randomKey];
        GameObject obj = currentPool.GetObject();

        obj.transform.position = GetRandomSpawnPoint();
        obj.transform.up = Vector3.zero - obj.transform.position;

        enemiesOnScreen++;

        if (obj.CompareTag("KillableEnemy"))
        {
            killableEnemiesOnScreen++;
            HelperMethods.AddEdibleBug(obj.transform);
        }

        if (currentPool.spawnTotal == 0)
        {
            keyList.Remove(randomKey);
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
        if (enemiesOnScreen > 0)
        {
            enemiesOnScreen--;

            if (killableEnemiesOnScreen > 0 && killableEnemies > 0)
            {
                killableEnemiesOnScreen--;
                killableEnemies--;
            }
        }

        if (isSpawing == false && killableEnemies > 0)
        {
            isSpawing = true;
        }

        if (killableEnemies == 0)
        {
            // TO DO: Running away logic from non killable enemies.
            EventList.noTargets.Invoke();
            isSpawing = false;
            keyList.Clear();
            EventList.waveChanged.Invoke();
            ReloadWaveData();
            return;
        }

    }

    IEnumerator WaveStartRoutine()
    {
        yield return waveStartCondition;
        isSpawing = true;
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
        }   
    }

    void EnemySpawn_OnEnemyLeft(string enemy)
    {
        spawnDictionary[enemy].spawnTotal++;
        if (spawnDictionary[enemy].prefab.CompareTag("KillableEnemy"))
        {
            killableEnemiesOnScreen--;
        }
        if (!keyList.Contains(enemy))
        {
            keyList.Add(enemy);
        }
        enemiesOnScreen--;       
    }
}
