using UnityEngine;

public class SpawnComponent : MonoBehaviour
{
    public GameObject DeathSpawn;
    public GameObject CaptureSpawn;

    public void SpawnOnDeath()
    {
        Spawn(DeathSpawn);
    }

    public void SpawnOnCapture()
    {
        Spawn(CaptureSpawn);
    }

    void Spawn(GameObject obj)
    {
        Instantiate(obj, transform.position, Quaternion.identity);
    }
}
