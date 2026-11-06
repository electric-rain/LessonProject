using System;
using UnityEngine;

public interface IEnemy
{
    event Action<IEnemy> OnEnemyDied;

    void Setup(EnemyData data);

    Transform transform { get; }
}
