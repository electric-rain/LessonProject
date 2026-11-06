using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

[CreateAssetMenu(fileName = "New Wave", menuName = "Game/Wave Data")]
public class WaveData : ScriptableObject
{
    public EnemyData[] EnemiesToSpawn;
    public float SpawnInterval = 0.5f;
}
