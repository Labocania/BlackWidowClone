using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour 
{
    public WaveData wave;
    public string insectName;
    UnityAction enemyDeath;
    InsectPool insectPool;
    Insect insect;
    int insectCount;

    void Awake()
    {
        insectPool = GetComponent<InsectPool>();
        ReadWaveData();
        enemyDeath += onEnemyDeath;
    }

    void Start()
    {
        insectPool.GetObject().Spawn(GetRandomSpawnPoint(), Quaternion.identity);
        EventBroker.StartListening("Enemy Death", enemyDeath);
    }

    void ReadWaveData()
    {
        insectPool.maximumSize = wave.enemyQuantity[insectName];
        insectCount = wave.enemyQuantity[insectName]; 
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
