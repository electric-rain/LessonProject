using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private int _score;

    private void Update()
    {
        _score = ScoreManager.Instance.Score;
    }
}
