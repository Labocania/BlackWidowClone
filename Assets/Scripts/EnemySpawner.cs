using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour 
{
    const int MAX_ENEMIES = 11;
    int enemiesOnScreen;
    public WaveData wave;
    UnityAction enemyDeath;
    Dictionary<GameObject, GameObjectPool> spawnDictionary = new Dictionary<GameObject, GameObjectPool>();

    void Awake()
    {
        enemyDeath += onEnemyDeath;
    }

    void Start()
    {
        ReadComponentData();
        ReadWaveData();
        EventBroker.StartListening("Enemy Death", enemyDeath);
    }

    void ReadComponentData()
    {
        foreach (var component in gameObject.GetComponents<GameObjectPool>())
        {
            spawnDictionary.Add(component.prefab, component);
        }
    }

    void ReadWaveData()
    {
        foreach (var enemy in wave.enemyQuantity)
        {
            spawnDictionary[enemy.Key].spawnCounter = enemy.Value;
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
