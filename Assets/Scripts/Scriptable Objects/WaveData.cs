using UnityEngine;

[CreateAssetMenu(fileName = "Wave Data")]
public class WaveData : ScriptableObject
{
    [Header("Web Color/Collider data")]
    [SerializeField]
    public WebDictionary webColliders;
    public EnemyDictionary enemyQuantity;

    //[HideInInspector]
    public int numberOfEnemies;
    public int waveNumber;

    void OnEnable()
    {
        foreach (var enemy in enemyQuantity)
        {
            numberOfEnemies += enemy.Value;
        }
    }
}
