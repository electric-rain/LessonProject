using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Data", menuName = "Game/Enemy Data")]
public class EnemyData : ScriptableObject
{
    public string EnemyName;
    public GameObject Prefab;
    public int MaxHealth;
    public float MoveSpeed;
    public float StopDistance;
    public int ScoreReward;
}



