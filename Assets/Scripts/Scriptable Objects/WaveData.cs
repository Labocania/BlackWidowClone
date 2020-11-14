using UnityEngine;

[CreateAssetMenu(fileName = "Wave Data")]
public class WaveData : ScriptableObject
{
    [Header("Web Color/Collider data")]
    [SerializeField]
    public WebDictionary webColliders;

    [Header("Enemy data")]
    public EnemyDictionary totalQuantity;
    public int waveNumber;
    public float spawingSpeed;
}
