using UnityEngine;

public class SpawnComponent : MonoBehaviour
{
    public GameObject ObjectToSpawn;
    public void Spawn()
    {
        Instantiate(ObjectToSpawn, transform.position, Quaternion.identity);
    }
}
